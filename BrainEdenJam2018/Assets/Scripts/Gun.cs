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

    [SerializeField] private Vector3 m_bulletOriginOffset;
   
    [SerializeField] private float m_fireRate = 0.25f;
    [SerializeField] private float m_maxShootingDistance;
    private float m_fireRateCooldown;

    [SerializeField] private float m_bulletDamage = 0.0f;
    [SerializeField] private float m_bulletSpeed;

    [SerializeField] private AudioClip m_shooting_audio;
    [SerializeField] private AudioClip m_reloading_audio;
    [SerializeField] private AudioSource m_audio_source;

    [SerializeField] private Transform m_cameraTransform;

    //Everything for reload and cllip size
    public int m_clipSize = 6;
    public int m_bulletsInClip = 6;
    public int m_totalAmmo = 24;
    [SerializeField] private float m_reloadTime = 2.0f;
    [SerializeField] private float m_repairTime = 6.0f;
    [SerializeField] private Image m_reloadBar;
    [SerializeField] private float m_percentComplete = 0;

    public bool m_isJammed = false;

    public float BulletSpeed {
        get { return m_bulletSpeed; }
        set { m_bulletSpeed = value; }
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
        get { return m_fireRate; }
        set { m_fireRate = value; }
    }

    public Vector3 Offset {
        get { return m_bulletOriginOffset; }
        set { m_bulletOriginOffset = value; }
    }

    void Start() 
	{
		//Lock and hide the mouse cursor 
		Cursor.lockState = CursorLockMode.Locked;

	}

	void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            GetComponent<UnreliableBehaviour>().FailChance = 15f;
            if (!GetComponent<UnreliableBehaviour>().HasFailed()) {
                if(m_fireRateCooldown <= 0.0f && m_bulletsInClip > 0 && !m_isJammed && m_totalAmmo > 0) {
                    StartCoroutine(Shoot());
                    m_bulletsInClip--;
                    m_totalAmmo--;
                    m_fireRateCooldown = m_fireRate;
                }
            }
            else if (GetComponent<UnreliableBehaviour>().HasFailed() && m_bulletsInClip > 0) {
                m_isJammed = true;
            }

            //IF GUN BACKFIRES
            GetComponent<UnreliableBehaviour>().FailChance = 10.0f;
            if (GetComponent<UnreliableBehaviour>().HasFailed() && !m_isJammed)
            {
                GetComponentInParent<Rigidbody>().AddExplosionForce(1000.0f, GetComponent<Transform>().position, 20.0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !m_isJammed && m_totalAmmo > 0)
        {
            StartCoroutine(Reload());
        }
        if (Input.GetKeyDown(KeyCode.R) && m_isJammed)
        {
            StartCoroutine(Repair());
        }

        m_fireRateCooldown -= Time.deltaTime;
        
        m_reloadBar.fillAmount = m_percentComplete;
    }

    public void AddAmmo(int amount) {
        m_totalAmmo += amount;
    }

    IEnumerator Repair() {
        float currentRepair = 0;
        m_reloadBar.color = Color.red;

        while (currentRepair < m_repairTime - 0.1f) {
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

        while (currentReload < m_reloadTime - 0.1f) {
            m_percentComplete = (currentReload / m_reloadTime);
            currentReload += Time.deltaTime;
            yield return null;
        }

        if(m_totalAmmo < m_clipSize) {
            m_bulletsInClip = m_totalAmmo;
        }
        else {
            m_bulletsInClip = m_clipSize;
        }
        m_reloadBar.fillAmount = 0;
        m_percentComplete = 0.0f;
    }


	IEnumerator Shoot()
	{
        //empty direction for use later
        Vector3 bulletDirection = Vector3.zero;
        Vector3 cameraRaycastOrigin = transform.position + transform.TransformDirection(Offset);

        RaycastHit raycastHit;

        if(Physics.Raycast(cameraRaycastOrigin, m_cameraTransform.forward, out raycastHit, m_maxShootingDistance)) {
            raycastHit.collider.gameObject.ToString();
            bulletDirection = raycastHit.point - (transform.position + m_bulletOriginOffset);
            if(Physics.Raycast(transform.position + m_bulletOriginOffset, bulletDirection, out raycastHit, m_maxShootingDistance)) {
                Enemy enemy = raycastHit.collider.gameObject.GetComponent<Enemy>();
                if (enemy) {
                    enemy.Damage(m_bulletDamage);
                }
            }
        }

        GetComponentInChildren<ParticleSystem>().Play();

        yield return null;
		
	}

}

	