using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputHandler : MonoBehaviour
{
    private GameControls _controls;
    
    private void Awake()
    {
        _controls = new GameControls();

        _controls.Default.Space.performed += HandleSpace;
        
        _controls.Enable();
    }

    private void HandleSpace(InputAction.CallbackContext context)
    {
        SceneHandler.SharedInstance.SwitchScene();
    }

    private void OnDestroy()
    {
        _controls.Default.Space.performed -= HandleSpace;
    }
}
