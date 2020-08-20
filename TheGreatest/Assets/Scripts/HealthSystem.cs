using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;
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
            else if (value < 0)
            {
                health = 0;
            }
            else
            {
                health = value;
            }
        }
    }

    private void Start()
    {
        Health = maxHealth;
    }

    public void Damage(int amount)
    {
        Health -= amount;
        Debug.Log(gameObject + " HealthSystem took " + amount + " damage and now has " + Health + " health left");
    }
}
