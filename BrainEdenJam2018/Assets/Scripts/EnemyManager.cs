using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> _EnemyPrefabs = new List<GameObject>();
    public List<Transform> _SpawnPoints = new List<Transform>();
    public float _MinRange;
    public float _MaxRange;
    public int _Quantity;
    public float Timer;
    private bool _Ready;

    public GameObject m_player;

    public List<GameObject> _Enemy = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
        if (_EnemyPrefabs.Count != 0 && _SpawnPoints.Count != 0)
        {
            _Ready = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < _Enemy.Count; i++)
        {
            if (_Enemy[i].GetComponent<Enemy>().IsDead)
            {
                GameObject enemy = _Enemy[i];
                _Enemy.RemoveAt(i);
                Destroy(enemy);
            }
        }
        if (_Enemy.Count < _Quantity && _Ready)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        _Ready = false;

        var offset = new Vector3(Random.Range(_MinRange, _MaxRange), 1.0f, Random.Range(_MinRange, _MaxRange));
        offset = _SpawnPoints[Random.Range(0, _SpawnPoints.Count)].TransformPoint(offset);

        var prefab = _EnemyPrefabs[Random.Range(0, _EnemyPrefabs.Count)];
        var newPlatform = Instantiate(prefab, offset, Quaternion.identity);
        newPlatform.transform.parent = transform;
        newPlatform.GetComponent<Enemy>().m_player = m_player;

        _Enemy.Add(newPlatform);

        yield return new WaitForSeconds(Timer);

        _Ready = true;
    }
}
