using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_NavMesh : MonoBehaviour {

    private Transform m_target;

    public Transform[] m_points;
    private int m_destination_point = 0;

    NavMeshAgent m_agent;


	// Use this for initialization
	void Start () {
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
            Collider[] hit_Colliders = Physics.OverlapSphere(transform.position, 10f);

            Vector3 player_pos = Vector3.zero;

            foreach (var item in hit_Colliders)
            {
                if (item.CompareTag("Player"))
                {
                    Debug.Log("PORCODIO");
                    player_pos = item.transform.position;
                }
            }
            if (player_pos != Vector3.zero)
            {
                if (Vector3.Dot((player_pos - transform.position).normalized, transform.forward) > 0.5f)
                {
                    Debug.Log("PORCODIO");
                }
            }
            
            if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
        }
    }



}
