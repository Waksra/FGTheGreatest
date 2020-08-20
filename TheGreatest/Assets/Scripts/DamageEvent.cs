using UnityEngine;
using UnityEngine.Events;

public class DamageEvent : MonoBehaviour
{
    public UnityEvent DealDamage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        DealDamage?.Invoke();
        collision.gameObject.SetActive(false);
    }

}
