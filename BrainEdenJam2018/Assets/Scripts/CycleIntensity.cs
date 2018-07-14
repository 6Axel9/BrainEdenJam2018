using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CycleIntensity : MonoBehaviour {

    //private Transform m_lightTransform;
    private float m_angle;

    [SerializeField]private float m_maxIntensity;

    public float MaxIntensity {
        get { return m_maxIntensity; }
        set { m_maxIntensity = value; }
    }

    // Use this for initialization
    void Start () {
        m_maxIntensity = GetComponent<Light>().intensity;
        m_angle = GetComponentInParent<DayLightCycle>().Angle;
    }
	
	// Update is called once per frame
	void Update () {

        //m_lightTransform = GetComponent<Transform>();
        m_angle = GetComponentInParent<DayLightCycle>().Angle;

        float modifier = 0.0f;
        if (CompareTag("Sun"))
        {
            modifier = 1.0f;
        }
        if (CompareTag("Moon"))
        {
            modifier = 0.1f;
        }
        

        //Moon Up
        if(m_angle > 0 && m_angle < 180)
        {
            if (GetComponent<Light>().intensity < m_maxIntensity)
            {
                GetComponent<Light>().intensity += Time.deltaTime * modifier;
            }
        }
        else
        {
            if (GetComponent<Light>().intensity > 0)
            {
                GetComponent<Light>().intensity -= Time.deltaTime * modifier;
            }
        }
	}
}
