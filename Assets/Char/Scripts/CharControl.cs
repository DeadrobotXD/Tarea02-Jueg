using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class CharControl : MonoBehaviour, Damage
{

    [SerializeField] private float Speed;
    private float currentSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float gravityFactor;
    [SerializeField] private float JumpValue;
    [SerializeField] private Movement Movement;
    [SerializeField] private Transform Camera;
    [SerializeField] private float fallDelay = 5f;
    [SerializeField] private float restartDelay = 5f;

    [Header("Combat")]
    [SerializeField] private float life;
    public float uiLife => life;
    [SerializeField] private float ShootRange;
    [SerializeField] private float damageValue;

    float airtime;
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
       Movement.OnSprint += Sprint;
       Movement.OnAttack += OnAttack;
       Movement.OnReload += OnReload;

    }

    private void OnDisable()
    {
        Movement.OnJump -= Jump;
        Movement.OnMove -= Move;
        Movement.OnSprint -= Sprint;

    }

    void Update()
    {
        moveDir = Camera.forward*moveInput.z + Camera.right * moveInput.x;
        moveDir.y = 0;
        characterController.Move(moveDir * currentSpeed * Time.deltaTime);
        anim.SetFloat("Speed", characterController.velocity.magnitude);
        if (moveDir.sqrMagnitude > 0)
        {
            Rotation();
        }
        bool isGrounded = Grounded();
        if (isGrounded) {
            airtime = 0f;
        }
        else {
            airtime += Time.deltaTime;
        }
            
        bool animGrounded = isGrounded || airtime < fallDelay;
        anim.SetBool("Grounded", animGrounded);
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
        currentSpeed = Speed;
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
       
    }

    private void Sprint(bool isSprinting)
    {
        currentSpeed = isSprinting ? sprintSpeed : Speed;
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

    private void OnAttack()
    {
        anim.SetTrigger("isShooting");
        if(Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, ShootRange))
        {
            if(hit.transform.gameObject.TryGetComponent<Damage>(out Damage damage))
            {
                damage.recieveDamage(damageValue);
            }
        }
    }
    private void OnReload()
    {
        anim.SetTrigger("Reload");

    }

    public void recieveDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
        
            anim.SetBool("Death", true);
            Movement.DisableInput();
            StartCoroutine(RestartAfterDelay());

        }
    }

    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

