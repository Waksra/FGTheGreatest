using System;
using UnityEngine;

namespace Actor_Components
{
    public class MovementController : MonoBehaviour
    {
        public float moveSpeed;
        public bool lockInBoundary = true;

        [Space(10)] public bool debug = false;
        
        private Transform _transform;

        private Vector2 _moveVector = Vector2.zero;

        private event Action<int> HorizontalDirectionChangedEvent; 

        public Vector2 MoveVector
        {
            get
            {
                return _moveVector;
            }

            set
            {
                if(Mathf.RoundToInt(value.x) != Mathf.RoundToInt(_moveVector.x))
                    HorizontalDirectionChangedEvent?.Invoke(Mathf.RoundToInt(value.x));
                
                _moveVector = value;
            }
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            Move(MoveVector);
            
            if (lockInBoundary)
                SceneBoundary.MoveBackToBounds(transform);
        }

        private void Move(Vector2 direction)
        {
            if(direction.sqrMagnitude > 1f)
                direction.Normalize();
        
            _transform.Translate(direction * (moveSpeed * Time.deltaTime));
        }

        public void SubscribeToHorizontalDirectionChangedEvent(Action<int> action)
        {
            HorizontalDirectionChangedEvent += action;
        }
        
        public void UnsubscribeToHorizontalDirectionChangedEvent(Action<int> action)
        {
            HorizontalDirectionChangedEvent -= action;
        }
    }
}
