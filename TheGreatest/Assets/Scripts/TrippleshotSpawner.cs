using UnityEngine;

public class TrippleshotSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPooler.PooledObjects objectToFire;
    [SerializeField] private Transform[] spawnpointArray;

    private void OnEnable()
    {
        for (int i = 0; i < spawnpointArray.Length; i++)
        {
            GameObject obj = ObjectPooler.SharedInstance.GetPooledObject(objectToFire);
            obj.transform.position = spawnpointArray[i].position;
            obj.transform.up = spawnpointArray[i].up;
            obj.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
