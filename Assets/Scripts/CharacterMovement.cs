using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSpeed;

    private float angle;
    private Quaternion targetRotation;
    private Vector2 input;
    private Vector3 movement;
    private CharacterController charController;
    private Animator charAnimator;

    private bool isRunning;

    private void Awake()
    {
        isRunning = false;
        charController = GetComponent<CharacterController>();
        charAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        PlayAnimation("Run", isRunning);
    }
    private void FixedUpdate()
    {
        GetInput();

        if (input.x != 0 || input.y != 0)
        {
            isRunning = true;
            CalculateRotationAngle();
            UpdateRotation();
            UpdatePosition();
        }
        else
        {
            isRunning = false;
        }
    }

    private void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void CalculateRotationAngle()
    {
        angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
    }

    private void UpdateRotation()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
    }

    private void UpdatePosition()
    {
        charController.Move(transform.forward * movementSpeed * Time.fixedDeltaTime);
    }

    private void PlayAnimation(string name, bool condition)
    {
        charAnimator.SetBool(name, condition);
    }
}
