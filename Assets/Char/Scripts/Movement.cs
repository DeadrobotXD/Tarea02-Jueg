using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Movement", menuName = "Scriptable Objects/Movement")]
public class Movement : ScriptableObject
{
    InputSystem_Actions CharInputSystem;
    public event Action OnJump;

    private void OnEnable()
    {
        CharInputSystem = new InputSystem_Actions();
        CharInputSystem.Player.Enable();
        CharInputSystem.Player.Jump.started += Jump_started;
        CharInputSystem.Player.Jump.canceled += Jump_canceled;
        Debug.Log("Listo");
    }

    private void Jump_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        
    }

    private void Jump_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Jump");
    }
}
