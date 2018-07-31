using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Enemy : MonoBehaviour, IMovement {

    private Animator m_anim;
    public Transform[] m_points;
    private float m_shooting_distance;
    private int m_destination_point = 0;

    [SerializeField] private float m_fireRate = 0.2f;
    private float m_fireRateCooldown;

    [SerializeField] private Transform player_pos;

    [SerializeField] private int m_clipSize = 6;
    [SerializeField] private int m_bulletsInClip = 6;
    [SerializeField] private float m_reloadTime = 10.0f;

    private bool m_reloadStarted = false;
    private bool m_canShoot = false;

    public GameObject m_player;
    private Rigidbody m_body;

    //TODO: Need some way to stop enemies from spinning during a reload cycle...
    //Just looks really strange that they stop shooting and the pirouette........

    NavMeshAgent m_agent;

    public bool m_isVillain;

    // Use this for initialization
    void Start () {

        m_anim = GetComponentInChildren<Animator>();
        m_agent = GetComponent<NavMeshAgent>();

        if (!m_agent)
        {
            Debug.Log("No Agent Found");
            return;
        }

        m_body = GetComponent<Rigidbody>();

        GotoNextPoint();
    }

    private void GotoNextPoint()
    {
        if (m_points.Length == 0)
        {
            return;
        }

        m_agent.destination = m_points[m_destination_point].position;
        m_destination_point = UnityEngine.Random.Range(0, m_points.Length);
    }

    
    // Update is called once per frame
    void Update () {
        if(Health <= 0.0f)
        {
            Kill();
            m_player.GetComponent<Humanoid>().Score++;
        }
        m_shooting_distance = 8.0f;

        if (m_agent != null && !IsDead)
        {
            Collider[] hit_Colliders = Physics.OverlapSphere(transform.position, 10.0f); //Got Rid of random overlap range

            player_pos = null;

            /*FOR EACH OBJECT IN A AREA OF 15 RADIUS AROUND THE NPC POSITION*/
            foreach (var item in hit_Colliders)
            {
                if (item.CompareTag("Player"))
                {
                    player_pos = item.transform;
                }
            }

            /*IF THE PLAYER IS FOUND*/
            if (player_pos)
            {

                /*IF THE NPC IS IN SHOOTING DISTANCE*/
                if (Vector3.Distance(player_pos.position, transform.position) < m_shooting_distance)
                {
                    /*AND THE PLAYER IS STILL IN SIGHT OF THE NPC*/
                    if (Vector3.Dot((player_pos.position - transform.position).normalized, transform.forward) > 0.6f)
                    {
                        m_agent.isStopped = true;
                        m_anim.SetBool("Shooting", true);

                        /*CHECKS TO SEE IF ANIMATION FOR SHOOOTING IS DONE BEFORE ACTUALLY SHOOTING*/
                        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("StartShooting"))
                        {
                            if(m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                            {
                                m_canShoot = true;
                            }
                        }

                        /*SHOOT NEARBY THE PLAYER IF ENEMY HAS BULLETS AND IS NOT ON COOLDOWN*/
                        if (m_fireRateCooldown <= 0.0f && m_bulletsInClip > 0 && m_canShoot)
                        {
                            StartCoroutine(Shoot(player_pos));
                            m_fireRateCooldown = m_fireRate;
                            m_bulletsInClip--;
                        }
                        if (m_bulletsInClip == 0 && !m_reloadStarted)
                        {
                            m_reloadStarted = true;
                            StartCoroutine(Reload());
                            m_canShoot = false;
                        }
                    }
                }
                else
                {
                    /*IF THE PLAYER IS IN FRONT OF THE PLAYER AND IS NOT SHOOTING*/
                    if (Vector3.Dot((player_pos.position - transform.position).normalized, transform.forward) > 0.5f)
                    {
                        Debug.Log("Going to player pos...");
                        m_agent.SetDestination(player_pos.position);
                        m_agent.isStopped = false;
                        m_anim.SetBool("Shooting", false);
                        m_canShoot = false;
                    }
                }
            }
            /*IF NONE OF THE ABOVE, CONTINUE YOUR BORING WALK TROUGH WAYPOINTS*/
            else if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
            {
                m_canShoot = false;
                Debug.Log("Getting a new path...");
                GotoNextPoint();
            }
            else
            {
                //Debug.Log("Just debugging");
            }
        }

        if (!m_agent.hasPath)
        {
            Debug.Log("Enemy has lost its path....");
        }

        m_fireRateCooldown -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if(m_body.velocity.magnitude > 0.1f)
        {
            m_anim.SetFloat("Velocity", m_body.velocity.magnitude);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
            IsFalling = false;
        }
    }

    IEnumerator Reload() {
        float currentReload = 0;
        while(currentReload < m_reloadTime - 0.1f)
        {
            currentReload += Time.deltaTime;
            yield return null;
        }
        m_bulletsInClip = m_clipSize;

        //This bool was added to stop co-routine from running concurrently underneath itself
        //this should stop the 6 shot revolver working like an uzi after reload.
        m_reloadStarted = false;
    }

    IEnumerator Shoot(Transform target)
    {
        Vector3 bulletSpawn = transform.position + transform.TransformDirection(new Vector3(0.0f, 0.0f, 1));
        Vector3 direction_to_player = target.position - transform.position;

        RaycastHit raycastHit;

        if(Physics.Raycast(bulletSpawn, direction_to_player, out raycastHit, m_shooting_distance))
        {
            Humanoid player = raycastHit.collider.gameObject.GetComponent<Humanoid>();
            if (player)
            {
                player.Damage(3.0f);
            }
        }

        GetComponentInChildren<ParticleSystem>().Play();

        yield return null;
    }
}
