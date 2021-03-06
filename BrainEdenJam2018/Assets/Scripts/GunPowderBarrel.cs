﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPowderBarrel : MonoBehaviour {

    [SerializeField] private float m_blastRadius;
    [SerializeField] private float m_blastForce;
    [SerializeField] private float m_damageCaused = 40.0f;

    private bool m_detonated = false;

    Collider[] m_colliderList;

	// Use this for initialization
	void Start () {
        m_colliderList = Physics.OverlapSphere(GetComponent<Transform>().position, m_blastRadius);
	}

	// Update is called once per frame
	void Update () {
        m_colliderList = Physics.OverlapSphere(GetComponent<Transform>().position, m_blastRadius);

        if(!GetComponentInChildren<ParticleSystem>().isPlaying && m_detonated)
        {
            Destroy(this.gameObject);
        }
    }

    public void Detonate() {
        m_detonated = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;

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
                isLiving = collider.gameObject.GetComponent<EnemyAI>();
                if (isLiving)
                {
                    collider.gameObject.GetComponent<EnemyAI>().Damage(m_damageCaused);
                }
            }
        }
        foreach(var component in GetComponentsInChildren<ParticleSystem>())
        {
            component.Play();
        }
    }
}
