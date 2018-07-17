using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [System.NonSerialized] private Vector3 m_speed;
    public float m_bulletDamage = 10f;
    private Rigidbody Body;
    private bool Shot;


    private GameObject m_bulletObject;

    [SerializeField] private float m_life_span;

    public GameObject BulletObject
    {
        get
        {
            return m_bulletObject;
        }

        set
        {
            m_bulletObject = value;
        }
    }

    public Vector3 Speed
    {
        get
        {
            return m_speed;
        }

        set
        {
            m_speed = value;
        }
    }

    public float LifeSpan
    {
        get
        {
            return m_life_span;
        }

        set
        {
            m_life_span = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        Body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Destroy(this.gameObject, m_life_span);
    }

    void FixedUpdate()
    {
        if (Body && !Shot)
        {
            Body.AddForce(m_speed);

            Shot = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

}
