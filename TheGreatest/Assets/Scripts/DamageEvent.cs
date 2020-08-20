using UnityEngine;
using UnityEngine.Events;

public class DamageEvent : MonoBehaviour
{
    public UnityEvent DealDamage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision registered");
        if (collision.CompareTag("PlayerBullet") && gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Tag found!");
            DealDamage?.Invoke();
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("EnemyBullet") && gameObject.CompareTag("Player"))
        {
            Debug.Log("Tag found!");
            DealDamage?.Invoke();
            collision.gameObject.SetActive(false);
        }
    }

}
