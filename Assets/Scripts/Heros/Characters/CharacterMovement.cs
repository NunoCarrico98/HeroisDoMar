using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement values")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSpeed;

    private float angle;
    private Quaternion targetRotation;
    private Vector2 positionInput;
    private Vector2 rotationInput;
    private Vector3 movement;
    private CharacterController charController;
    private Animator charAnimator;

    private Vector3 verticalMove;
    private bool isGrounded;
    private float verticalVelocity;

    private bool isRunning;
    private int pNumber;

    public bool IsMovementAllowed { get; set; }
    public float MovementSpeed
    {
        get => movementSpeed;
        set { movementSpeed = value; }
    }

    private void Awake()
    {
        isRunning = false;
        charController = GetComponent<CharacterController>();
        charAnimator = GetComponentInChildren<Animator>();
        pNumber = GetComponent<Hero>().PlayerNumber;
        IsMovementAllowed = true;
    }

    public void Move()
    {
        PlayAnimation("Run", isRunning);

        positionInput.x = InputManager.PositionHorizontal(pNumber);
        positionInput.y = InputManager.PositionVertical(pNumber);
        rotationInput.x = InputManager.RotationHorizontal(pNumber);
        rotationInput.y = InputManager.RotationVertical(pNumber);

        if (positionInput.x != 0 || positionInput.y != 0 && IsMovementAllowed)
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
        isGrounded = charController.isGrounded;

        if (isGrounded)
        {
            verticalVelocity -= 0;
        }
        else
        {
            verticalVelocity -= 1;
        }

        verticalMove = new Vector3(0, verticalVelocity, 0);
        charController.Move(verticalMove);

        charController.Move(moveInput * movementSpeed * Time.fixedDeltaTime);
    }

    private void PlayAnimation(string name, bool condition)
    {
        charAnimator.SetBool(name, condition);
    }
}
