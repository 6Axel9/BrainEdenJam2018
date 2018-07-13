using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBrain : MonoBehaviour
{

    /// <summary>
    /// Variables to hold states of FSM, used to swap a Coroutine (state) with another.
    /// </summary>
	IEnumerator _currState;
    IEnumerator _nextState;

    public List<Vector3> m_steerings = new List<Vector3>();

    void Start()
    {
        ///First coroutine will always be the move state
		_currState = Moving();
        StartCoroutine(StateMachine());
    }

    /// <summary>
    /// Moving state will have Avoid Input and Wander Input
    /// </summary>
	IEnumerator Moving()
    {
        Debug.Log("There");

        yield return null;
    }

    /// <summary>
    /// Flock state will have avoid input, follow to follow the leader and separation to keep distance from other members of the flock
    /// </summary>
	IEnumerator Flock()
    {
        yield return null;
    }

    /// <summary>
    /// Attack will only be entered from leaders and will seek at full speed the player
    /// </summary>
	IEnumerator Attack()
    {
        yield return null;
    }

    /// <summary>
    /// Idling is actually just a standby for 1.5 to 3 seconds, then the agent will go back to moving
    /// </summary>
	IEnumerator Idling()
    {
        while (_nextState == null)
        {

            yield return new WaitForSeconds(Random.Range(1.5f, 3));
            _nextState = Moving();
            yield return null;
        }
    }

    /// <summary>
    /// The actual StateMachine will handle the state swap and loop
    /// </summary>
	IEnumerator StateMachine()
    {
        while (_currState != null)
        {
            yield return StartCoroutine(_currState);
            _currState = _nextState;
            _nextState = null;
        }
    }

    /// <summary>
    /// All the collision is checked in here and states are swapped accordingly
    /// </summary>
	private void OnTriggerEnter(Collider coll)
    {
        
    }

    /// <summary>
    /// If an enemy is near the agent but is not flocking, then dont add it to the neighbors count
    /// Otherwise do add it.
    /// </summary>
	private void OnTriggerStay(Collider coll)
    {

    }

    /// <summary>
    /// If the leader has left, top flocking and start Moving around
    /// </summary>
	private void OnTriggerExit(Collider coll)
    {
        
    }

}
