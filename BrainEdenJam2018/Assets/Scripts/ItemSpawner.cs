using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public List<Transform> m_spawnPoints = new List<Transform>();

    public GameObject m_ammo;
    public GameObject m_tonic;

    public float m_timer;
    public bool m_spawnAmmo;
    public bool m_spawnTonic;

    public Transform m_ammoLocation;
    public Transform m_tonicLocation;
    

	// Use this for initialization
	void Start () {
        m_spawnAmmo = true;
        m_spawnTonic = true;
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

        if (!m_ammo)
        {
            m_spawnAmmo = true;
        }
        if (!m_tonic)
        {
            m_spawnTonic = true;
        }
    }

    IEnumerator SpawnAmmo()
    {
        m_ammoLocation = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        while(m_ammoLocation == m_tonicLocation)
        {
            m_ammoLocation = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        }
        GameObject AmmoBox = Instantiate(m_ammo, m_ammoLocation.position, Quaternion.identity);

        yield return null;
    }

    IEnumerator SpawnTonic()
    {
        m_tonicLocation = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        while (m_tonicLocation == m_ammoLocation)
        {
            m_tonicLocation = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        }
        GameObject AmmoBox = Instantiate(m_ammo, m_tonicLocation.position, Quaternion.identity);

        yield return null;
    }
}
