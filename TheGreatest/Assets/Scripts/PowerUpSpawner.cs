using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private float minTimeToSpawn;
    [SerializeField] private float maxTimeToSpawn;
    private float _timeToSpawn;
    [SerializeField] private ObjectPooler.PooledObjects powerUpToSpawn;

    private void Start()
    {
        SetTimeToSpawn();
    }

    private void Update()
    {
        if (_timeToSpawn > 0f)
        {
            _timeToSpawn -= Time.deltaTime;
        }
        else
        {
            SpawnPowerUp();
            SetTimeToSpawn();
        }
    }

    private void SpawnPowerUp()
    {
        GameObject obj = ObjectPooler.SharedInstance.GetPooledObject(powerUpToSpawn);
        obj.SetActive(true);
    }

    private void SetTimeToSpawn()
    {
        _timeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
    }
}
