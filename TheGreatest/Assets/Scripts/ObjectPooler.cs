using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;
}
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    public List<ObjectPoolItem> itemsToPool;
    [SerializeField] private List<GameObject> pooledObjects;
    
    public enum PooledObjects
    {
        PlayerBullet,
        EnemyBullet,
        TrippleShot,
        Enemy
    }

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }        
    }

    public GameObject GetPooledObject(PooledObjects tagEnum)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].CompareTag(GetTagFromEnum(tagEnum)))
            {
                return pooledObjects[i];
            }
        }

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.CompareTag(GetTagFromEnum(tagEnum)))
            {
                if (item.shouldExpand)
                {
                    GameObject obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }

    private string GetTagFromEnum(PooledObjects tagEnum)
    {
        switch (tagEnum)
        {
            case PooledObjects.PlayerBullet:
                return "PlayerBullet";
            case PooledObjects.EnemyBullet:
                return "EnemyBullet";
            case PooledObjects.TrippleShot:
                return "TrippleShot";
            case PooledObjects.Enemy:
                return "Enemy";
        }

        return "";
    }
}
