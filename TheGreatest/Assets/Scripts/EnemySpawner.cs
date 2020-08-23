using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public Vector2 enemySpawnDelayRange;
    
    private Transform[] _spawns;

    private float _nextEnemyDelay = 5;
    private void Awake()
    {
        _spawns = GetComponentsInChildren<Transform>();
        Debug.Log(_spawns[0].position);
    }

    private void Update()
    {
        if (_nextEnemyDelay <= 0)
        {
            SpawnEnemy();
            _nextEnemyDelay = Random.Range(enemySpawnDelayRange.x, enemySpawnDelayRange.y);
        }
        else
        {
            _nextEnemyDelay -= Time.deltaTime;
        }
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = ObjectPooler.SharedInstance.GetPooledObject(ObjectPooler.PooledObjects.Enemy);
        if (newEnemy == null)
            return;

        int spawnLocation = Random.Range(1, _spawns.Length - 1);
        newEnemy.transform.position = _spawns[spawnLocation].position;
        newEnemy.SetActive(true);
    }
}
