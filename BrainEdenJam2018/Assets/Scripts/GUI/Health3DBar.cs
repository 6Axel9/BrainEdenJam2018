using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health3DBar : MonoBehaviour {

    private float MaxHealth;
    private float Health;
    private EnemyAI EnemyScript;

	// Use this for initialization
	void Start () {

        EnemyScript = GetComponentInParent<EnemyAI>();

        MaxHealth = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {

        Health = EnemyScript.Health/100 * MaxHealth;

        if(Health < 0.0f)
        {
            Health = 0.0f; ;
        }
        transform.localScale = new Vector3(Health, transform.localScale.y, transform.localScale.z);
	}
}
