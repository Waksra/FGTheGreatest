using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private float minTimeToSpawn;
    [SerializeField] private float maxTimeToSpawn;
    private float timeToSpawn;
    [SerializeField] private ObjectPooler.PooledObjects powerUpToSpawn;

    private void Start()
    {
        SetTimeToSpawn();
    }

    private void Update()
    {
        if (timeToSpawn > 0f)
        {
            timeToSpawn -= Time.deltaTime;
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
        timeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
    }
}
