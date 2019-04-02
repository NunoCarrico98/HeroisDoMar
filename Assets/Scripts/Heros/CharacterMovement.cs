using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSpeed;

    private float angle;
    private Quaternion targetRotation;
    private Vector2 positionInput;
    private Vector2 rotationInput;
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
        GetPositionInput();
        GetRotationInput();

        if (positionInput.x != 0 || positionInput.y != 0)
        {
            isRunning = true;
            UpdatePosition(new Vector3(positionInput.x, 0, positionInput.y));
        }
        else
        {
            isRunning = false;
        }

        if (rotationInput.x != 0 || rotationInput.y != 0)
        {
            CalculateRotationAngle();
            UpdateRotation(new Vector3(rotationInput.x, 0, rotationInput.y));
        }
    }

    private void GetPositionInput()
    {
        positionInput.x = Input.GetAxisRaw("Horizontal");
        positionInput.y = Input.GetAxisRaw("Vertical");
    }

    private void GetRotationInput()
    {
        rotationInput.x = Input.GetAxisRaw("RightHorizontal");
        rotationInput.y = Input.GetAxisRaw("RightVertical");
    }

    private void CalculateRotationAngle()
    {
        angle = Mathf.Atan2(rotationInput.x, rotationInput.y) * Mathf.Rad2Deg;
    }

    private void UpdateRotation(Vector3 lookDirectionInput)
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
    }

    private void UpdatePosition(Vector3 moveInput)
    {
        charController.Move(moveInput * movementSpeed * Time.fixedDeltaTime);
    }

    private void PlayAnimation(string name, bool condition)
    {
        charAnimator.SetBool(name, condition);
    }
}
