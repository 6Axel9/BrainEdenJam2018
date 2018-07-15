﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPowderBarrel : MonoBehaviour {

    [SerializeField] private float m_blastRadius;
    [SerializeField] private float m_blastForce;
    [SerializeField] private float m_damageCaused = 5.0f;

    Collider[] m_colliderList;

	// Use this for initialization
	void Start () {
        m_colliderList = Physics.OverlapSphere(GetComponent<Transform>().position, m_blastRadius);
	}

	// Update is called once per frame
	void Update () {
        m_colliderList = Physics.OverlapSphere(GetComponent<Transform>().position, m_blastRadius);
    }

    public void Detonate() {
        foreach(var collider in m_colliderList)
        {
            if (collider.CompareTag("Explodable") || collider.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(m_blastForce, GetComponent<Transform>().position, m_blastRadius);
                Component isLiving = collider.gameObject.GetComponent<Humanoid>();
                if (isLiving)
                {
                    collider.gameObject.GetComponent<Humanoid>().Damage(m_damageCaused);
                }
                isLiving = collider.gameObject.GetComponent<Enemy>();
                if (isLiving)
                {
                    collider.gameObject.GetComponent<Enemy>().Damage(m_damageCaused);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Detonate();
            Destroy(this.gameObject);
        }
    }
}
