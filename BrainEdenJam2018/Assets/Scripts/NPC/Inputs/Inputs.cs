using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour {
	public Transform _target; 
	protected Transform last_rot;
	protected Transform _origin;

	public virtual Vector3 Steering {
		get{ 
			return Vector3.zero;
		}
	}

	void Start () {
		 
		_origin = GetComponent<Transform> ();
	}
		
}
