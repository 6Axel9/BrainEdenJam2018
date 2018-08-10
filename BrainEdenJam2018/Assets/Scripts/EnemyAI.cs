using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IMovement, ILiving<float> {

    private Animator m_animator;
    private NavMeshAgent m_agent;
    private Rigidbody m_body;
    public GameObject m_player;

    //NAVIGATION
    private int m_destIndex = 0;
    private Transform[] m_pathPoints;
    private Transform m_target;
    private Vector3 m_previousDest = Vector3.zero;
    [SerializeField] private float m_awareDistance = 10f;

    public Transform[] PathPoints {
        get { return m_pathPoints; }
        set { m_pathPoints = value; }
    }

    //SHOOTING DATA
    [SerializeField] private float m_gunDamage = 3f;
    [SerializeField] private float m_fireRate = .2f;
    [SerializeField] private float m_reloadTime = 10f;
    [SerializeField] private float m_shootingDistance = 8f;
    [SerializeField] private int m_clipSize = 6;
    private int m_bulletsInClip = 6;
    private float m_fireRateCooldown;

    //CO-ROUTINE CONTROLS
    private bool m_canReload = true;
    private bool m_canShoot = false;

    //MOVEMENT DATA
    private bool m_jumping;
    private bool m_falling;

    public bool IsJumping {
        get { return m_jumping; }
    }

    public bool IsFalling {
        get { return m_falling; }
    }

    //LIVING DATA
    [SerializeField] private float m_health = 100f;
    [SerializeField] private float m_maxHealth = 100f;
    private bool m_isDead = false;


    // Use this for initialization
    void Start () {
        m_animator = GetComponentInChildren<Animator>();
        if (!m_animator) {
            Debug.Log("No Animator Found in EnemyAI.cs");
            return;
        }
        m_agent = GetComponent<NavMeshAgent>();
        if (!m_agent) {
            Debug.Log("No Agent Found In EnemyAI.cs !");
            return;
        }
        m_body = GetComponent<Rigidbody>();
        if (!m_body)        {
            Debug.Log("No RigidBody Found in EnemyAI.cs !");
            return;
        }
        if (!GoToNextPoint()) {
            Debug.Log("Can't get new destination at EnemyAI.cs Start!");
        }
	}
	
	// Update is called once per frame
	void Update () {
        //If Enemy is dead increment score.
        if (IsDead()) {
            m_player.GetComponent<Humanoid>().Score++;
        }

        if(m_agent && !IsDead())
        {
            //Get a list of objects nearby the enemy.
            Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, m_awareDistance);

            //Set the target to null so target is recalculated each frame.
            m_target = null;

            //Loop through all nearby objects searching for the player.
            foreach(var obj in nearbyObjects) {
                if (obj.CompareTag("Player")) {
                    m_target = obj.transform;
                }
            }

            //If the player is found
            if(m_target != null)
            {
                //If the player is within shooting distance.
                if(Vector3.Distance(m_target.position, transform.position) <= m_shootingDistance) {
                    //Stop the agent from walking
                    m_agent.isStopped = true;
                    //Make agent face player whilst shooting.
                    m_agent.transform.forward = m_target.position - transform.position;
                    //Start the shooting animation
                    m_animator.SetBool("Shooting", true);

                    //Get the current animation state info.
                    AnimatorStateInfo animationInfo = m_animator.GetCurrentAnimatorStateInfo(0);

                    //If the animation has finished playing, shooting can start.
                    if (animationInfo.IsName("StartShooting")) {
                        if(animationInfo.normalizedTime > 1.0f) {
                            m_canShoot = true;
                        }
                    }
                    //If enemy can shoot, then shoot.
                    if(m_fireRate <= 0.0f && m_bulletsInClip > 0 && m_canShoot) {
                        StartCoroutine(Shoot(m_target));
                        m_fireRateCooldown = m_fireRate;
                        m_bulletsInClip--;
                    }
                    //If bullet clip is empty then reload.
                    if(m_bulletsInClip == 0 && m_canReload) {
                        m_canReload = false;
                        StartCoroutine(Reload());
                        m_canShoot = false;
                    }
                }
                //If not within shooting distance.
                else {
                    //Get previous destination.
                    m_previousDest = m_agent.destination;
                    //Set destination to player.
                    m_agent.SetDestination(m_target.position);
                    //Allow agent to walk again.
                    m_agent.isStopped = false;
                    //Stop the shooting animation.
                    m_animator.SetBool("Shooting", false);
                    //Stop Enemy from shooting.
                    m_canShoot = false;
                }
            }
            //If the player is not found.
            else {
                //If enemy has a previously interrupted destination
                if (m_previousDest != Vector3.zero) {
                    //Then go back to using that destination.
                    m_agent.destination = m_previousDest;
                    //Set previous destination holder to zero.
                    m_previousDest = Vector3.zero;
                }
                //If still going to a waypoint but reached destination
                else if(!m_agent.pathPending && m_agent.remainingDistance < 0.5f) {
                    //Turn of shooting just incase
                    m_canShoot = false;
                    //Get a new destination and path to it.
                    if (!GoToNextPoint()) {
                        Debug.Log("Could not get new destination in EnemyAI.cs !");
                    }
                }
                //Allow agent to start running again.
                m_agent.isStopped = false;
                //Stop enemy shooting
                m_canShoot = false;
            }
        }
        //Increment fire rate cooldown.
        m_fireRateCooldown -= Time.deltaTime;
	}

    void FixedUpdate() {
        //Once the enemy starts moving
        if(m_body.velocity.magnitude > 0.1f) {
            //Increase the animation play speed to match the body walk speed.
            m_animator.SetFloat("Velocity", m_body.velocity.magnitude);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            m_jumping = false;
            m_falling = false;
        }
    }

    //NAVIGATION
    private bool GoToNextPoint() {
        //Check to see if there are any points in the enemy path.
        if(PathPoints.Length == 0) {
            Debug.Log("No Path Points In EnemyAI.cs !");
            return false;
        }
        //If there are points then assign one ass enemy destination.
        if (!m_agent.SetDestination(PathPoints[m_destIndex].position)) {
            Debug.Log("Could Not Set Enemy Destination In EnemyAI.cs !");
            return false;
        }
        //If a destination was set then change the destination index for a new point next iteration.
        m_destIndex = Random.Range(0, PathPoints.Length - 1);
        return true;
    }

    //MOVEMENT
    public void Crouch(bool crouch) {
        //UNUSED ATM
    }

    public void Jump(float force) {
        m_jumping = true;
        m_body.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    public void Strafe(float velocity) {
        m_body.AddForce(transform.right * velocity);
    }

    public void Walk(float velocity) {
        Vector3 forward = transform.forward;
        forward.y = 0;
        m_body.AddForce(forward * velocity);
    }

    public void Fall(float force) {
        m_falling = true;
        m_body.AddForce(Vector3.down * force);
    }

    //LIVING
    public void Kill() {
        m_isDead = true;
    }

    public void Heal(float amount) {
        m_health += amount;
        if(m_health > m_maxHealth) {
            m_health = m_maxHealth;
        }
    }

    public void Damage(float amount) {
        m_health -= amount;
    }

    public bool IsDead() {
        if(m_health <= 0.0f) {
            m_isDead = true;
        }
        return m_isDead;
    }

    //CO-ROUTINES
    IEnumerator Shoot(Transform target) {
        //Get where bullet starts
        Vector3 bulletSpawn = transform.position + transform.TransformDirection(new Vector3(0.0f, 0.0f, 1f));
        //Get direction to player.
        Vector3 directionToPlayer = target.position - transform.position;

        //TODO: Add bullet shooting wobble so it doesnt always hit player.

        //Structure to hold raycast hit data.
        RaycastHit raycastHit;

        //Check to see if enemy hit player.
        if(Physics.Raycast(bulletSpawn, directionToPlayer, out raycastHit, m_shootingDistance)) {
            //If raycast hits player.
            Humanoid player = raycastHit.collider.gameObject.GetComponent<Humanoid>();
            if (player) {
                //Hurt player.
                player.Damage(m_gunDamage);
            }
        }

        //After damaging player, play gun muzzle flash.
        GetComponentInChildren<ParticleSystem>().Play();

        //End Co-routine.
        yield return null;
    }

    IEnumerator Reload() {
        //Reload time elapsed, starts at 0;
        float currentReload = 0f;
        //While still reloading
        while(currentReload < m_reloadTime - .1f) {
            //Increment time elapsed.
            currentReload += Time.deltaTime;
            //Exit co-routine until next iteration.
            yield return null;
        }
        //Reset number of bullets in clip.
        m_bulletsInClip = m_clipSize;
        //Allow future reloads.
        m_canReload = true;
    }

   
}
