using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Movement", menuName = "Scriptable Objects/Movement")]
public class Movement : ScriptableObject
{
    InputSystem_Actions CharInputSystem;
    public event Action OnJump;
    public event Action OnAttack;
    public event Action OnReload;
    public event Action<Vector2> OnMove;
    public event Action<bool> OnSprint;


    

    private void OnEnable()

    {
        CharInputSystem = new InputSystem_Actions();
        CharInputSystem.Player.Enable();
        CharInputSystem.Player.Jump.started += Jump;
        CharInputSystem.Player.Attack.started += Attack;
        CharInputSystem.Player.Interact.started += Reload;
        CharInputSystem.Player.Move.performed += Move;
        CharInputSystem.Player.Move.canceled += Move;
        CharInputSystem.Player.Sprint.started += SprintStart;
        CharInputSystem.Player.Sprint.canceled += SprintEnd;
    }
    void OnDisable()
    {
        CharInputSystem.Player.Move.performed -= Move;
        CharInputSystem.Player.Move.canceled -= Move;
        CharInputSystem.Player.Disable();

    }

    void OnDestroy()
    {
        CharInputSystem.Player.Disable();
        CharInputSystem.Dispose();
    }  

    private void Jump(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke();
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        OnAttack?.Invoke();
    }

    private void Reload(InputAction.CallbackContext ctx)
    {
        OnReload?.Invoke();
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }
    private void SprintStart(InputAction.CallbackContext ctx)
    {
        OnSprint?.Invoke(true);
        Debug.Log("Corro");
    }

    private void SprintEnd(InputAction.CallbackContext ctx)
    {
        OnSprint?.Invoke(false);

    }
    public void DisableInput()
    {
        CharInputSystem.Player.Disable();
        OnMove?.Invoke(Vector2.zero);
        OnSprint?.Invoke(false);
    }
}
