using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MovementManager : MonoBehaviour {
	
	public List <Inputs> _steerings = new List <Inputs>();
    
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
			
		if (_steerings.Count > 0) {
			Vector3 FinalSteer = Vector3.zero;

			foreach (var steering in _steerings) {
				FinalSteer += steering.Steering;
			}
								
            GetComponent<Rigidbody>().AddForce(FinalSteer * GetComponent<Enemy>().m_moveSpeed);
		}
	}

	public void AddSteering(Inputs v){
		_steerings.Add (v);
	}
	public void RemoveSteering(Inputs v){
		if (_steerings.Contains(v)) {
			_steerings.Remove (v);
		}
	}
	public void ClearSteerings(){
		_steerings.Clear();
	}
}
