using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnreliableBehaviour : MonoBehaviour {

    [SerializeField] private float m_failChance;
    [SerializeField] private float m_modifier = 1;

    public float FailChance {
        get { return m_failChance; }
        set { m_failChance = value; }
    }

    public bool HasFailed()
    {
        if(Random.Range(0,100) < FailChance * m_modifier)
        {
            return true;
        }
        return false;
    }

	// Use this for initialization
	void Start () {
    }
}
