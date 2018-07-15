using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Enemy : MonoBehaviour, IMovement
{

    private Animator m_anim;
    private Rigidbody m_body;

    public float m_animSpeed;
    public float m_moveSpeed;
    public float m_jumpForce;


    // Use this for initializationw
    void Start () {
        m_body = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (IsDead)
        {
            Destroy(this.gameObject);
        }

        if (Health <= 0.0f)
        {
            Kill();
        }
    }

    void FixedUpdate()
    {
        if(m_body.velocity.magnitude > 0.1f)
        {
            m_anim.SetFloat("Velocity", m_body.velocity.magnitude);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
            IsFalling = false;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Damage(collision.gameObject.GetComponent<Bullet>().m_bulletDamage);
        }
    }


}
