using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_NavMesh : MonoBehaviour {

    private Animator m_anim;
    public Transform[] m_points;
    private float m_shooting_distance;
    private int m_destination_point = 0;

    NavMeshAgent m_agent;

    [SerializeField] private GameObject m_bullet;
    public bool m_isVillan;

    public GameObject Bullet
    {
        get { return m_bullet; }
        set { m_bullet = value; }
    }

    // Use this for initialization
    void Start () {

        m_anim = GetComponentInChildren<Animator>();


        m_agent = this.GetComponent<NavMeshAgent>();

        if (!m_agent)
        {
            Debug.Log("No Agent Found");
            return;
        }

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

        m_shooting_distance = UnityEngine.Random.Range(6, 8);

        if (m_agent != null)
        {
            Collider[] hit_Colliders = Physics.OverlapSphere(transform.position, UnityEngine.Random.Range(10, 15));

            Transform player_pos = null;

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
                        /*SHOOT NEARBY THE PLAYER*/
                        StartCoroutine(Shoot(player_pos));
                    }
                }
                else
                {
                    /*IF THE PLAYER IS IN FRONT OF THE PLAYER AND IS NOT SHOOTING*/
                    if (Vector3.Dot((player_pos.position - transform.position).normalized, transform.forward) > 0.5f)
                    {
                        m_agent.SetDestination(player_pos.position);
                        m_agent.isStopped = false;
                        m_anim.SetBool("Shooting", false);
                    }
                }

                /*IF NONE OF THE ABOVE, CONTINUE YOUR BORING WALK TROUGH WAYPOINTS*/
                if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
                {
                    GotoNextPoint();
                }

            }
        }

    }


    IEnumerator Shoot(Transform target)
    {
        Vector3 bulletSpawn = transform.position + transform.TransformDirection(new Vector3(0.0f, 0.0f, 1));

        Vector3 direction_to_player = target.position - transform.position;

        //Create a new bullet
        GameObject newBullet = Instantiate(Bullet, bulletSpawn, Quaternion.identity);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction_to_player), 5);

        //Give it speed
        newBullet.GetComponent<Bullet>().Speed = 5000 * direction_to_player.normalized;

        yield return null;
    }
}
