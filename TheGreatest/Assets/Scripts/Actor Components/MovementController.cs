using UnityEngine;

namespace Actor_Components
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;
        
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        public void Move(Vector2 direction)
        {
            if(direction.sqrMagnitude > 1f)
                direction.Normalize();
        
            _transform.Translate(direction * moveSpeed);
        }
    }
}
