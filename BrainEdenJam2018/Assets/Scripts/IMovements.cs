using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    void Strafe(float velocity);
    void Walk(float velocity);
    void Jump(float force);
    void Crouch(bool crouch);
    void Fall(float force);
}

public partial class Humanoid
{
    private bool Jumping;
    private bool Falling;

    public bool IsJumping
    {
        get
        {
            return Jumping;
        }

        set
        {
            Jumping = value;
        }
    }

    public bool IsFalling
    {
        get
        {
            return Falling;
        }

        set
        {
            Falling = value;
        }
    }

    public void Strafe(float speed)
    {
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        Body.AddForce(transform.right * speed);
    }

    public void Walk(float speed)
    {
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        Vector3 forward = (transform.forward);
        forward.y = 0;
        Body.AddForce(forward * speed);
    }

    public void Jump(float force)
    {
        IsJumping = true;

        //Anim.SetTrigger("Jump");
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        Body.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    public void Fall(float force)
    {
        IsFalling = true;

        //Anim.SetTrigger("Jump");
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        Body.AddForce(Vector3.down * force);

    }

    public void Crouch(bool crouch)
    {
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        //Anim.SetBool("Crouch", crouch);

        Debug.Log("Crouch");
    }
}

public partial class Enemy
{
    private bool Jumping;
    private bool Falling;

    public bool IsJumping
    {
        get
        {
            return Jumping;
        }

        set
        {
            Jumping = value;
        }
    }

    public bool IsFalling
    {
        get
        {
            return Falling;
        }

        set
        {
            Falling = value;
        }
    }

    public void Strafe(float speed)
    {
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        m_body.AddForce(transform.right * speed);
    }

    public void Walk(float speed)
    {
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        Vector3 forward = (transform.forward);
        forward.y = 0;
        m_body.AddForce(forward * speed);
    }

    public void Jump(float force)
    {
        IsJumping = true;

        //Anim.SetTrigger("Jump");
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        m_body.AddForce(Vector3.up * force, ForceMode.Impulse);

        Debug.Log("Jump");
    }

    public void Fall(float force)
    {
        IsFalling = true;

        //Anim.SetTrigger("Jump");
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        m_body.AddForce(Vector3.down * force);

        Debug.Log("Fall");
    }

    public void Crouch(bool crouch)
    {
        //Anim.SetFloat("Speed", Body.velocity.magnitude);
        //Anim.SetBool("Crouch", crouch);

        Debug.Log("Crouch");
    }
}