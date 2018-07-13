using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [System.NonSerialized] private Vector3 m_speed;

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

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(m_speed);
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

}
