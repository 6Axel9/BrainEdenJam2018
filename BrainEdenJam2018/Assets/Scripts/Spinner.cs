using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

    public bool m_xAxis;
    public bool m_yAxis;
    public bool m_zAxis;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_xAxis)
        {
            transform.Rotate(Time.deltaTime * 10, 0, 0);
        }
        if (m_yAxis)
        {
            transform.Rotate(0,Time.deltaTime * 10, 0);
        }
        if (m_zAxis)
        {
            transform.Rotate(0,0,Time.deltaTime * 10);
        }
    }
}
