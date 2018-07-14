using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_NavMesh : MonoBehaviour {

    public Transform m_target;

    NavMeshAgent m_agent;


	// Use this for initialization
	void Start () {
        m_agent = this.GetComponent<NavMeshAgent>();
        if (!m_agent)
        {
            Debug.Log("No Aagent Found");
            return;
        }

	}

    private void SetDirection()
    {
        if (m_target)
        {
            Vector3 t_destination;

            t_destination = m_target.transform.position;

            m_agent.SetDestination(t_destination);
        }
    }

    // Update is called once per frame
    void Update () {
        if (m_agent)
        {
            SetDirection();
        }
    }
}
