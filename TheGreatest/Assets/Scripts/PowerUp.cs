using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PowerUp : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float powerUpDuration;
    public ObjectPooler.PooledObjects objectToFire;
    public UnityEvent powerUpEvent;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float startPosY;
    private Vector3 startPosition;
    private float startPosX;
    [SerializeField] private float sinWidth;
    [SerializeField] private float sinSpeed;

    private void OnEnable()
    {
        startPosX = Random.Range(-5f, 5f);
        startPosition = new Vector3(startPosX, startPosY, 0f);
    }
    private void Update()
    {
        transform.position = startPosition + new Vector3(Mathf.Sin(Time.time * sinSpeed) * sinWidth, startPosition.y += movementSpeed * Time.deltaTime, 0f);

        if (transform.position.y <= -startPosY)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            powerUpEvent?.Invoke();
            collision.gameObject.GetComponent<Actor_Components.ShootingController>().ChangeProjectile(objectToFire, powerUpDuration);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
