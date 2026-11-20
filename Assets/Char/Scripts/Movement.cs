using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Movement", menuName = "Scriptable Objects/Movement")]
public class Movement : ScriptableObject
{
    InputSystem_Actions CharInputSystem;
    public event Action OnJump;
    public event Action<Vector2> OnMove;


    private void OnEnable()
    {
        CharInputSystem = new InputSystem_Actions();
        CharInputSystem.Player.Enable();
        CharInputSystem.Player.Jump.started += Jump;
        CharInputSystem.Player.Move.performed += Move;
        CharInputSystem.Player.Move.canceled += Move;
        Debug.Log("Listo");
    }
    void OnDisable()
    {
        CharInputSystem.Player.Move.performed -= Move;
        CharInputSystem.Player.Move.canceled -= Move;
        CharInputSystem.Player.Disable();
    }

    void OnDestroy()
    {
        CharInputSystem.Dispose();
    }  

    private void Jump(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke();
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }
}
