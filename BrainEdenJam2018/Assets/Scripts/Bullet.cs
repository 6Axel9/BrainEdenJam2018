using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [System.NonSerialized] private Vector3 m_speed;

    [SerializeField] private float m_life_span;

    private GameObject m_bulletObject;

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
        GetComponent<Rigidbody>().AddForce(m_speed);
    }

    void Update()
    {
        Destroy(this.gameObject, m_life_span);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

}
