﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public UnityEvent Death;
    public UnityEvent AfterDeath;
    [SerializeField] private int maxHealth;
    [SerializeField] private ObjectPooler.PooledObjects deathEffect;

    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value > maxHealth)
            {
                health = maxHealth;
            }
            else if (value <= 0)
            {
                health = 0;
                Die();
            }
            else
            {
                health = value;
            }
        }
    }

    private void Awake()
    {
        Health = maxHealth;
    }

    public void Damage(int amount)
    {
        Health -= amount;
        Debug.Log($"{gameObject} HealthSystem took {amount.ToString()} damage and now has {Health.ToString()} health left");
    }

    private void Die()
    {
        GameObject obj = ObjectPooler.SharedInstance.GetPooledObject(deathEffect);
        obj.transform.position = transform.position;
        obj.SetActive(true);
        AfterDeath?.Invoke();
        gameObject.SetActive(false);
        Health = maxHealth;
    }
}
