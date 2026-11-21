using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharControl : MonoBehaviour
{

    [SerializeField] private float Speed;
    [SerializeField] private float gravityFactor;
    [SerializeField] private float JumpValue;
    [SerializeField] private Movement Movement;
    [SerializeField] private Transform Camera;
    private Animator anim;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private Vector3 moveInput;
    [SerializeField] private Vector3 verticalSpeed;
    private CharacterController characterController;
    [SerializeField] private Transform feet;
    [SerializeField] private float DetectRadius;
    [SerializeField] private LayerMask floor;



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
        if (moveDir.sqrMagnitude > 0)
        {
            Rotation();
        }
        bool isGrounded = Grounded();
        anim.SetBool("Grounded", isGrounded);
        if(isGrounded && verticalSpeed.y < 0)
        {
            verticalSpeed.y = 0;
            anim.ResetTrigger("isJumping");
        }
        ApplyGravity();
      

    }

    private void ApplyGravity()
    {
        verticalSpeed.y += gravityFactor * Time.deltaTime;
        characterController.Move(verticalSpeed * Time.deltaTime);
    }

    private bool Grounded()
    {
        return Physics.CheckSphere(feet.position, DetectRadius, floor);
    }

    private void Move(Vector2 ctx)
    {
        moveInput = new Vector3(ctx.x, 0, ctx.y);
        
    }

    private void Jump()
    {
        if (Grounded())
        {
            verticalSpeed.y = Mathf.Sqrt(-2 * gravityFactor * JumpValue);
            anim.SetTrigger("isJumping");
        }
        
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
       
    }
    

     private void Rotation()
    {
        Quaternion rotationTarget = Quaternion.LookRotation(moveDir);
        transform.rotation = rotationTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feet.position, DetectRadius);

    }
}

