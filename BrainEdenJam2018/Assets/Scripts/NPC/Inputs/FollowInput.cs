using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInput : Inputs {

	public override Vector3 Steering {
		get{
			if (GetComponent<EnemyBrain>().Leader != null) {
				return Steerings.Follow(_origin, GetComponent<EnemyBrain>().Leader);
			}

			else{
				return Vector3.zero;
			}
		}
	}
}
