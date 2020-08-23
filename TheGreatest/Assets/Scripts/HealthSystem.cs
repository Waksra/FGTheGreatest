using UnityEngine;
using UnityEngine.Events;


public class HealthSystem : MonoBehaviour
{
    public UnityEvent deathEvent;
    [SerializeField] private int maxHealth;
    [SerializeField] private ObjectPooler.PooledObjects deathEffect;

    private int _health;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            if (value > maxHealth)
            {
                _health = maxHealth;
            }
            else if (value <= 0)
            {
                _health = 0;
                Die();
            }
            else
            {
                _health = value;
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
        deathEvent?.Invoke();
        gameObject.SetActive(false);
        Health = maxHealth;
    }
}
