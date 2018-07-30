using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public List<Transform> m_spawnPoints = new List<Transform>();

    public GameObject m_ammoPrefab;
    public GameObject m_tonicPrefab;

    public GameObject m_spawnedAmmo;
    public GameObject m_spawnedTonic;

    public float m_timer;
    public bool m_spawnAmmo;
    public bool m_spawnTonic;

    public Transform m_ammoLocation;
    public Transform m_tonicLocation;
    

	// Use this for initialization
	void Start () {
        m_spawnAmmo = true;
        m_spawnTonic = true;

        m_spawnedAmmo = null;
        m_spawnedTonic = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_spawnAmmo)
        {
            StartCoroutine(SpawnAmmo());
        }
        if (m_spawnTonic)
        {
            StartCoroutine(SpawnTonic());
        }

        if (!m_spawnedAmmo)
        {
            m_spawnAmmo = true;
        }
        if (!m_spawnedTonic)
        {
            m_spawnTonic = true;
        }
    }

    IEnumerator SpawnAmmo()
    {
        m_spawnAmmo = false;
        m_ammoLocation = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        while(m_ammoLocation == m_tonicLocation)
        {
            m_ammoLocation = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        }
        m_spawnedAmmo = Instantiate(m_ammoPrefab, m_ammoLocation.position, Quaternion.identity);

        yield return null;
    }

    IEnumerator SpawnTonic()
    {
        m_spawnTonic = false;
        m_tonicLocation = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        while (m_tonicLocation == m_ammoLocation)
        {
            m_tonicLocation = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        }
        m_spawnedTonic = Instantiate(m_tonicPrefab, m_tonicLocation.position, Quaternion.identity);


        yield return null;
    }
}
