using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationInput : Inputs {
	public override Vector3 Steering {
		get{
			Vector3 final_separation = Vector3.zero;
			if (GetComponent<EnemyBrain> ().Neighbors.Count > 0 ) {

				final_separation = Steerings.Separate (_origin, GetComponent<EnemyBrain> ().Neighbors);
									
				return final_separation;
			} else {
				return Vector3.zero;
			}

		}
	}
}
