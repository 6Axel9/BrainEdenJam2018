using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int m_maximumEnemies;
    [SerializeField] private float m_minimumOffset;
    [SerializeField] private float m_maximumOffset;
    [SerializeField] private float m_spawnTimer;
    [SerializeField] private List<GameObject> m_enemyPrefabs = new List<GameObject>();
    [SerializeField] private List<Transform> m_spawnPoints = new List<Transform>();

    [SerializeField] private Transform[] m_wayPoints;

    private bool m_canSpawnEnemy = true;
    private GameObject m_player;
    private List<GameObject> m_enemyList = new List<GameObject>();

    void Start ()
    {
        //Make sure there is a list of enemy types and spawn points
        if (m_enemyPrefabs.Count == 0 && m_spawnPoints.Count == 0) {
            Debug.Log("No Enemy Prefabs or Spawn Points!");
        }
        //Get a reference to the player for later use.
        m_player = GameObject.FindGameObjectWithTag("Player");
        if (!m_player) {
            Debug.Log("Could Not Find Player Object!");
        }
        //Attempt to get all spawn points for enemies by getting transform components from children.
        //Skip the first component as this is the transform above the children.
        Transform[] childSpawnPoints = GetComponentsInChildren<Transform>();
        for(int i = 1; i < childSpawnPoints.Length; i++) {
            m_spawnPoints.Add(childSpawnPoints[i]);
        }
    }
    
    void Update() {
        //For each enemy in the list
        for(int i = 0; i < m_enemyList.Count; i++) {
            //Check to see if enemy has died
            if (m_enemyList[i].GetComponent<EnemyAI>().IsDead()) {
                //If enemy has died then get a reference to it.
                GameObject enemy = m_enemyList[i];
                //Remove it from the enemy list.
                m_enemyList.RemoveAt(i);
                //Then destroy the game object.
                Destroy(enemy);
            }
        }
        //If the maximum enemy count has not ben reached, spawn new enemy.
        if (m_enemyList.Count < m_maximumEnemies && m_canSpawnEnemy) {
            m_canSpawnEnemy = false;
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy() {
        //Spawn Timer elapsed time.
        float currentSpawnTime = 0f;
        //While waiting for spawn timer.
        while(currentSpawnTime < m_spawnTimer) {
            //Increment time elapsed.
            currentSpawnTime += Time.deltaTime;
            //Exit Co-routine until next iteration.
            yield return null;
        }

        //Finished spawn point after apply offset.
        Transform spawnPoint = null;
        //Offset of spawn point
        Vector3 offset = Vector3.zero;

        //Flag to say calculated spawn is ok to use.
        bool spawnOK = false;
        while (!spawnOK) {
            //Pick a spawn point from the list.
            spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count - 1)];
            //Create an offset.
            offset = new Vector3(Random.Range(m_minimumOffset, m_maximumOffset), 1.0f, Random.Range(m_minimumOffset, m_maximumOffset));
            //Check if the new spot is inside a building.
            Collider[] colliders = Physics.OverlapSphere(spawnPoint.position + offset, 1f);
            //Set spawnOK to true
            spawnOK = true;
            //If any colliders are a building set spawnOk to false;
            foreach (var collider in colliders) {
                if (collider.gameObject.CompareTag("Building")) {
                    spawnOK = false;
                }
            }
        }
        //If spawn is ok then instantiate enemy. First choice an enemy type.
        var enemyPrefab = m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Count - 1)];
        //Instantiate the new enemy at the calculated spawn point.
        var newEnemy = Instantiate(enemyPrefab, spawnPoint.position + offset, Quaternion.identity);
        newEnemy.transform.parent = transform;
        //Assign the player to the enemy for score purposes.
        newEnemy.GetComponent<EnemyAI>().Player = m_player;
        //Assign a waypoint list to enemy.
        newEnemy.GetComponent<EnemyAI>().PathPoints = m_wayPoints;
        //Store the new enemy is enemylist.
        m_enemyList.Add(newEnemy);

        m_canSpawnEnemy = true;
    }
}
