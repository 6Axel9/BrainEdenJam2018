using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health3DBar : MonoBehaviour {

    private float MaxHealth;
    private float Health;
    private Enemy EnemyScript;

	// Use this for initialization
	void Start () {

        EnemyScript = GetComponentInParent<Enemy>();

        MaxHealth = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {

        Health = EnemyScript.Health/100 * MaxHealth;

        transform.localScale = new Vector3(Health, transform.localScale.y, transform.localScale.z);
	}
}
