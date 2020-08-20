using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int bulletSpeed;

    private void Update()
    {
        rb.velocity = transform.up * bulletSpeed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!rb.IsTouchingLayers(1 << 10))
        {            
            gameObject.SetActive(false);
        }
    }
}
