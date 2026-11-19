using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharControl : MonoBehaviour
{

    [SerializeField] private float Speed;
    public float MoveHor;
    public float gravity;
    public float MoveVer;
    [SerializeField] private Vector3 verticalVelocity;
    private CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (!characterController.isGrounded)
        {
            applyGravity();
        }
    }

    // Update is called once per frame
    void Update()
    {
        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity* Time.deltaTime);
    }

    private void applyGravity()
    {
        
    }
}
