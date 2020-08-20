﻿using System;
using UnityEngine;

namespace Actor_Components
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;
        
        private Transform _transform;

        public Vector2 MoveVector { get; set; } = Vector2.zero;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            Move(MoveVector);
        }

        private void Move(Vector2 direction)
        {
            if(direction.sqrMagnitude > 1f)
                direction.Normalize();
        
            _transform.Translate(direction * (moveSpeed * Time.deltaTime));
        }
    }
}