using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Humanoid : MonoBehaviour, IMovement
{
    private Animator Anim;
    private Rigidbody Body;

    public float AnimationSpeed;
    public float MovementSpeed;
    public float JumpForce;
    public float FallForce;

    // Use this for initialization
    void Start () {

        Body = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(m_health <= 0.0f)
        {
            m_isDead = true;
        }
    }

    void FixedUpdate () {

        if (Input.GetKey(KeyCode.Space) && !IsJumping)
        {
            Jump(JumpForce);
        }
        else if (!Input.GetKey(KeyCode.Space) && IsJumping)
        {
            Fall(FallForce);
        }
        else if (Body.velocity.y < 0.2f || IsFalling)
        {
            if(IsJumping)
            {
                Fall(FallForce);
            }
        }

        Walk(MovementSpeed * Input.GetAxis("Vertical"));
        Strafe(MovementSpeed * Input.GetAxis("Horizontal"));

        if (Input.GetKey(KeyCode.F))
        {
            Crouch(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
            IsFalling = false;
        }
    }
}
