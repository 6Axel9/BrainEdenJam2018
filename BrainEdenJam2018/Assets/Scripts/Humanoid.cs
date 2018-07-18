using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using UnityEngine;

public partial class Humanoid : MonoBehaviour, IMovement, IInteraction
{
    private Animator Anim;
    private Rigidbody Body;

    public float AnimationSpeed;
    public float MovementSpeed;
    public float JumpForce;
    public float FallForce;
    public float SwapForce;
    public float ThrowForce;
    public float CollectSpeed;

    public int Score;
    private bool buttonPressed;

    public Image m_healthBar;
    public float m_percentageHealth = 0;

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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        m_percentageHealth = (m_health / m_maxHealth);
        m_healthBar.fillAmount = m_percentageHealth;
    }

    void FixedUpdate () {

        if (Input.GetMouseButtonDown(1) && Holding)
        {
            Throw(ThrowForce);
        }
        if (Input.GetKeyDown(KeyCode.Q) && Holding)
        {
            Swap(SwapForce);
        }


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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Crouch(true);
        }

        Pickup(CollectSpeed);

        if ((Input.GetKeyDown(KeyCode.E) && !Holding) || Holding)
        {
            Hold(CollectSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
            IsFalling = false;
        }

        if (collision.gameObject.CompareTag("Pickup"))
        {
            var script = collision.gameObject.GetComponent<Pickup>();
            if(script.isAmmo)
            {
                GetComponentInChildren<PlayerGun>().AddAmmo(script.Quantity);
            }
            if (!script.isAmmo)
            {
                Heal(script.Quantity);
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            m_health -= 0.5f;
        }
    }
}
