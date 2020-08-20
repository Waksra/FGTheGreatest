using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor_Components
{
    public class PlayerBrain : MonoBehaviour
    {
        private GameControls _controls;
        private MovementController _movementController;

        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            
            _controls = new GameControls();
            _controls.Default.Move.performed += HandleMove;
            _controls.Default.Move.canceled += HandleMove;
            _controls.Enable();
        }

        private void HandleMove(InputAction.CallbackContext context)
        {
            _movementController.MoveVector = context.ReadValue<Vector2>();
        }
    }
}
