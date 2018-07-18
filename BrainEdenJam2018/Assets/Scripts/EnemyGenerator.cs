using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public List<GameObject> _EnemyPrefabs = new List<GameObject>();
    public List<Transform> _SpawnPoints = new List<Transform>();
    public float _MinRange;
    public float _MaxRange;
    public int _Quantity;
    public float Timer;
    private bool _Ready;

    private List<GameObject> _Enemy = new List<GameObject>();

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

        _Enemy.Add(newPlatform);

        yield return new WaitForSeconds(Timer);

        _Ready = true;
    }
}
