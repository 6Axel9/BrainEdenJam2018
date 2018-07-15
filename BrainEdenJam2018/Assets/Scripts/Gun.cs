using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Gun : MonoBehaviour
{
    /// <summary>
    /// This Class is exlusively for shooting, it handles audio, direction and speed of the bullet. 
    /// </summary>

    [SerializeField] private Vector3 m_offset;
   
    [SerializeField] private float m_fire_rate = 0.25f;
    [SerializeField] private float m_cooldown;

    [SerializeField] private Transform m_arm;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private float m_bullet_speed;

    [SerializeField] private AudioClip m_shooting_audio;
    [SerializeField] private AudioClip m_reloading_audio;


    [SerializeField] private AudioSource m_audio_source;

    [SerializeField] private Transform m_camera;

    //Everything for reload and cllip size
<<<<<<< HEAD
    public int m_clipSize = 6;
    public int m_bulletsInClip = 6;
    public int m_totalAmmo = 24;
    [SerializeField] private float m_reloadTime = 2.0f;
    [SerializeField] private float m_repairTime = 6.0f;
    [SerializeField] private Image m_reloadBar;
    [SerializeField] private float m_percentComplete = 0;

    public bool m_isJammed = false;

=======
    [SerializeField] private int m_clipSize = 6;
    [SerializeField] private int m_bulletsInClip = 6;
    [SerializeField] private float m_reloadTime = 5.0f;
    [SerializeField] private Image m_reloadBar;
    [SerializeField] private float m_percentComplete = 0;

>>>>>>> master
    public Transform Arm {
        get { return m_arm; }
        set { m_arm = value; }
    }

    public GameObject Bullet {
        get { return m_bullet; }
        set { m_bullet = value; }
    }

    public float BulletSpeed {
        get { return m_bullet_speed; }
        set { m_bullet_speed = value; }
    }

    public AudioClip ShootingAudio {
        get { return m_shooting_audio; }
        set { m_shooting_audio = value; }
    }

    public AudioClip ReloadingAudio {
        get { return m_reloading_audio; }
        set { m_reloading_audio = value; }
    }

    public AudioSource AudioSource {
        get { return m_audio_source; }
        set { m_audio_source = value; }
    }

    public float FireRate {
        get { return m_fire_rate; }
        set { m_fire_rate = value; }
    }

    public Vector3 Offset {
        get { return m_offset; }
        set { m_offset = value; }
    }

    void Start() 
	{
		//Lock and hide the mouse cursor 
		Cursor.lockState = CursorLockMode.Locked;

	}

	void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<UnreliableBehaviour>().FailChance = 15f;
            if (!GetComponent<UnreliableBehaviour>().HasFailed())
            {
<<<<<<< HEAD
                if(m_cooldown <= 0.0f && m_bulletsInClip > 0 && !m_isJammed && m_totalAmmo > 0)
                {
                    StartCoroutine(Shoot());
                    m_bulletsInClip--;
                    m_totalAmmo--;
=======
                if(m_cooldown <= 0.0f && m_bulletsInClip > 0)
                {
                    StartCoroutine(Shoot());
                    m_bulletsInClip--;
>>>>>>> master
                    m_cooldown = m_fire_rate;
                }
            }
            else if (GetComponent<UnreliableBehaviour>().HasFailed() && m_bulletsInClip > 0)
            {
                m_isJammed = true;
            }

            //IF GUN BACKFIRES
            GetComponent<UnreliableBehaviour>().FailChance = 10.0f;
            if (GetComponent<UnreliableBehaviour>().HasFailed() && !m_isJammed)
            {
                GetComponentInParent<Rigidbody>().AddExplosionForce(1000.0f, GetComponent<Transform>().position, 20.0f);
            }
        }

<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.R) && !m_isJammed && m_totalAmmo > 0)
        {
            StartCoroutine(Reload());
        }
        if (Input.GetKeyDown(KeyCode.R) && m_isJammed)
        {
            StartCoroutine(Repair());
        }
=======
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
>>>>>>> master

        m_cooldown -= Time.deltaTime;
        
        m_reloadBar.fillAmount = m_percentComplete;
        //m_reloadBar.fillAmount += Time.deltaTime;
    }

<<<<<<< HEAD
    public void AddAmmo(int amount)
    {
        m_totalAmmo += amount;
    }

    IEnumerator Repair()
    {
        float currentRepair = 0;
        m_reloadBar.color = Color.red;

        while (currentRepair < m_repairTime - 0.1f)
        {

            m_percentComplete = (currentRepair / m_repairTime);

            currentRepair += Time.deltaTime;

            yield return null;

        }

        m_isJammed = false;
        m_reloadBar.fillAmount = 0;
        m_percentComplete = 0.0f;
    }

    IEnumerator Reload() {
        float currentReload = 0;
        m_reloadBar.color = Color.green;
=======
    IEnumerator Reload() {
        float currentReload = 0;
>>>>>>> master

        while (currentReload < m_reloadTime - 0.1f)
        {

            m_percentComplete = (currentReload / m_reloadTime);
            
            currentReload += Time.deltaTime;

            yield return null;
            
        }

<<<<<<< HEAD
        if(m_totalAmmo < m_clipSize)
        {
            m_bulletsInClip = m_totalAmmo;
        }
        else
        {
            m_bulletsInClip = m_clipSize;
        }
=======
        m_bulletsInClip = m_clipSize;
>>>>>>> master
        m_reloadBar.fillAmount = 0;
        m_percentComplete = 0.0f;

    }


	IEnumerator Shoot()
	{

        //if (!m_audio_source.isPlaying) {

        //Vector3 crosshair = m_camera.position + m_camera.forward * 55.0f;

        Vector3 direction = Vector3.zero;
        Vector3 bulletSpawn = m_arm.position + m_arm.TransformDirection(Offset);

        Vector3 raycast = (m_camera.position + m_camera.forward * 30.0f) - bulletSpawn;

        RaycastHit[] hitObjects = Physics.RaycastAll(m_camera.position, m_camera.forward);

        if(hitObjects.Length > 0)
        {
            Vector3 hitPoint = hitObjects[0].point;
            direction = hitPoint - bulletSpawn;
        }
        else
        {
            direction = raycast.normalized;
        }

        //Create a new bullet
        GameObject newBullet = Instantiate (m_bullet, bulletSpawn, Quaternion.identity);

            m_audio_source.PlayOneShot (m_shooting_audio);

			//Give it speed
			newBullet.GetComponent<Bullet> ().Speed = m_bullet_speed * direction.normalized;

            yield return null;

            //m_audio_source.PlayOneShot (m_reloading_audio);

		//}
		
	}

}

	