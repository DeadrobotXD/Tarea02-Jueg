using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharControl : MonoBehaviour
{

    [SerializeField] private float Speed;
    [SerializeField] private Movement Movement;
    [SerializeField] private Transform Camera;
    private Animator anim;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private Vector3 moveInput;
    [SerializeField] private Vector3 verticalVelocity;
    private CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
       Movement.OnJump += Jump;
       Movement.OnMove += Move;

    }

    void Update()
    {
        moveDir = Camera.forward*moveInput.z + Camera.right * moveInput.x;
        moveDir.y = 0;
        characterController.Move(moveDir * Speed * Time.deltaTime);
        anim.SetFloat("Speed", characterController.velocity.magnitude);
        //verticalVelocity.y += gravity * Time.deltaTime;
        //characterController.Move(verticalVelocity* Time.deltaTime);
    }
    
    private void Move(Vector2 ctx)
    {
        moveInput = new Vector3(ctx.x, 0, ctx.y);
        Rotation();
    }

    private void Jump()
    {
        Debug.Log("PersnajeSalta");
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
       
    }
    

     private void Rotation()
    {
        Quaternion rotationTarget = Quaternion.LookRotation(moveDir);
        transform.rotation = rotationTarget;
    }
}

