using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Shooting : MonoBehaviour
{
    /// <summary>
    /// This Class is exlusively for shooting, it handles audio, direction and speed of the bullet. 
    /// </summary>

    [SerializeField] private float m_offset;
   
    [SerializeField] private float m_fire_rate;

    [SerializeField] private Transform m_arm;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private float m_bullet_speed;

    [SerializeField] private AudioClip m_shooting_audio;
    [SerializeField] private AudioClip m_reloading_audio;


    [SerializeField] private AudioSource m_audio_source;

    public Transform Arm
    {
        get
        {
            return m_arm;
        }

        set
        {
            m_arm = value;
        }
    }

    public GameObject Bullet
    {
        get
        {
            return m_bullet;
        }

        set
        {
            m_bullet = value;
        }
    }

    public float BulletSpeed
    {
        get
        {
            return m_bullet_speed;
        }

        set
        {
            m_bullet_speed = value;
        }
    }

    public AudioClip ShootingAudio
    {
        get
        {
            return m_shooting_audio;
        }

        set
        {
            m_shooting_audio = value;
        }
    }

    public AudioClip ReloadingAudio
    {
        get
        {
            return m_reloading_audio;
        }

        set
        {
            m_reloading_audio = value;
        }
    }

    public AudioSource AudioSource
    {
        get
        {
            return m_audio_source;
        }

        set
        {
            m_audio_source = value;
        }
    }

    public float FireRate
    {
        get
        {
            return m_fire_rate;
        }

        set
        {
            m_fire_rate = value;
        }
    }

    public float Offset
    {
        get
        {
            return m_offset;
        }

        set
        {
            m_offset = value;
        }
    }

    void Start() 
	{
		//Lock and hide the mouse cursor 
		Cursor.lockState = CursorLockMode.Locked;

	}

	void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
        {
		    StartCoroutine(FireBullet());
        }
	}


	IEnumerator FireBullet()
	{
		
		//if (!m_audio_source.isPlaying) {
			//Create a new bullet
			GameObject newBullet = Instantiate (m_bullet, m_arm.position + m_arm.TransformDirection(Offset, 0.0f, 0.0f), m_arm.rotation);

            m_audio_source.PlayOneShot (m_shooting_audio);

			//Give it speed
			newBullet.GetComponent<Bullet> ().Speed = m_bullet_speed * m_arm.right;

			yield return new WaitForSeconds (m_fire_rate);

            //m_audio_source.PlayOneShot (m_reloading_audio);

		//}
		
	}

}

	