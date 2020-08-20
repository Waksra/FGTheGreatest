using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float startPosY;
    [SerializeField] private float respawnTimer;
    [SerializeField] private float timeToRespawn;

    void Start()
    {
        transform.position = new Vector3(RandomizeXPos(), startPosY, 0f);
    }

    void Update()
    {
        if(timeToRespawn > 0f)
        {
            timeToRespawn -= Time.deltaTime;
        }
        else
        {
            transform.position = transform.position + new Vector3(0f, -moveSpeed * Time.deltaTime, 0f);
        }

        if (transform.position.y <= -startPosY)
        {
            timeToRespawn = respawnTimer;
            transform.position = new Vector3(RandomizeXPos(), startPosY, 0f);
        }
    }

    private float RandomizeXPos()
    {
        float x = Random.Range(-10f, 10f);
        return x;
    }
}
