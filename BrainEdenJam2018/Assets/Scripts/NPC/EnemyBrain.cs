using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyBrain : MonoBehaviour {

    /// <summary>
    /// Variables to hold states of FSM, used to swap a Coroutine (state) with another.
    /// </summary>
	IEnumerator m_currState;
	IEnumerator m_nextState;

    /// <summary>
    /// A List of transform of all the closest enemies
    /// </summary>
	[SerializeField] private List <Transform> m_neighbors = new List <Transform>();

    /// <summary>
    /// The transform of the Leader to be followed when flocking
    /// </summary>
	private Transform m_leader;

    /// <summary>
    /// Maximum number of a single flock
    /// </summary>
	private int m_max_flock = 15;



	public List <Transform> Neighbors{		
		get { return m_neighbors; }
	}

	public Transform Leader{		
		get { return m_leader; }
	}

	void Start () {
        ///First coroutine will always be the move state
		m_currState = Moving ();	
		StartCoroutine(StateMachine()); 
	}

    /// <summary>
    /// Moving state will have Avoid Input and Wander Input
    /// </summary>
	IEnumerator Moving(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<WanderInput>());
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;

        while (m_nextState == null) {
			if (Input.GetKeyDown(KeyCode.Tab)) {
				m_nextState = Alert ();
			}
            if (m_neighbors.Count > 0)
            {
                m_nextState = Flock();
            }
            yield return null;
		}
	}

    /// <summary>
    /// Alert State will be entered when a bullet has been shot nearby
    /// </summary>
	IEnumerator Alert(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<EvadeInput>());
		while (m_nextState == null) {
           

            yield return null;
		}
	}

    /// <summary>
    /// Flock state will have avoid input, follow to follow the Leader and separation to keep distance from other members of the flock
    /// </summary>
	IEnumerator Flock(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<FollowInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<SeparationInput>());
		while (m_nextState == null) {
            Debug.Log("FLOCKING");
			if (m_neighbors.Count > m_max_flock || m_leader == null) {
				m_leader = null;
				m_neighbors.Clear();
				m_nextState = Idling (); //change state
			}
			
			yield return null;
		}

	}

    /// <summary>
    /// Attack will only be entered from leaders and will seek at full speed the player
    /// </summary>      
	IEnumerator Attack(){

		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<SeekInput>());
		gameObject.GetComponentInChildren<Renderer> ().material.color = Color.red;

		while (m_nextState == null) {

            if (Input.GetKeyDown(KeyCode.Return))
            {
                m_nextState = Moving();
            }

            yield return null;
		}

	}

    /// <summary>
    /// Idling is actually just a standby for 1.5 seconds, then the agent will go back to moving
    /// </summary>
	IEnumerator Idling(){
		GetComponent<MovementManager> ().ClearSteerings ();
		while (m_nextState == null) {

			yield return new WaitForSeconds (1.5f);
			m_nextState = Moving ();
			yield return null;
		}
	}

    /// <summary>
    /// The actual StateMachine will handle the state swap and loop
    /// </summary>
	IEnumerator StateMachine(){
		while (m_currState != null) {
			yield return StartCoroutine (m_currState);
			m_currState = m_nextState;
			m_nextState = null;
		}
	}

    /// <summary>
    /// All the collision is checked in here and states are swapped accordingly
    /// </summary>
	private void OnTriggerEnter(Collider coll){
		if (coll.tag != null) {
            if (!gameObject.CompareTag("Leader"))
            {
                if (coll.CompareTag("Enemy"))
                {
                    m_neighbors.Add(coll.transform);
                }
                else if (coll.CompareTag("Leader"))
                {
                    m_leader = coll.transform;
                }
            }
            else
            {

            }
		}
	}

    /// <summary>
    /// If an enemy is near the agent but is not flocking, then dont add it to the Neighbors count
    /// Otherwise do add it.
    /// </summary>
	private void OnTriggerStay(Collider coll){

        //if (gameObject.CompareTag("Enemy"))
        //{
        //    if (coll.CompareTag("Enemy"))
        //    {
        //        if (coll.GetComponent<EnemyBrain>().Leader == null)
        //        {
        //            if (m_neighbors.Contains(coll.transform))
        //            {
        //                m_neighbors.Remove(coll.transform);
        //            }
        //        }
        //    }
        //}
    }

    /// <summary>
    /// If the Leader has left, top flocking and start Moving around
    /// </summary>
	private void OnTriggerExit(Collider coll)
    {
        if (coll.tag != null)
        {
            if (m_neighbors.Contains(coll.transform))
            {
                m_neighbors.Remove(coll.transform);
            }
        }
    }
}
