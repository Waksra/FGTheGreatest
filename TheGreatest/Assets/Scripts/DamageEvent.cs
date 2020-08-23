using UnityEngine;
using UnityEngine.Events;

public class DamageEvent : MonoBehaviour
{
    public UnityEvent dealDamage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        dealDamage?.Invoke();
        collision.gameObject.SetActive(false);
    }

}
