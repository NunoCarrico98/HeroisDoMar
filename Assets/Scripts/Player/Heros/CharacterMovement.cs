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
	private Vector3 lastPosition;
	private float velocity;

	private Vector3 verticalMove;
	private float verticalVelocity;

	private int pNumber;

	public bool IsMovementAllowed { get; set; }
	public bool IsSlowed { get; set; }
	public float MovementSpeed
	{
		get => movementSpeed;
		set { movementSpeed = value; }
	}

	public void SetupCharacterMovement(int pNumber, Animator charAnimator)
	{
		charController = GetComponent<CharacterController>();
		this.charAnimator = charAnimator;
		this.pNumber = pNumber;
		IsMovementAllowed = true;
		IsSlowed = false;
	}

	public void Move()
	{
		lastPosition = transform.position;

		positionInput.x = InputManager.PositionHorizontal(pNumber);
		positionInput.y = InputManager.PositionVertical(pNumber);
		rotationInput.x = InputManager.RotationHorizontal(pNumber);
		rotationInput.y = InputManager.RotationVertical(pNumber);

		if (IsMovementAllowed)
		{
			if (positionInput.x != 0 || positionInput.y != 0)
			{
				UpdatePosition(new Vector3(positionInput.x, 0, positionInput.y));
			}

			if (rotationInput.x != 0 || rotationInput.y != 0)
			{
				CalculateRotationAngle();
				UpdateRotation(new Vector3(rotationInput.x, 0, rotationInput.y));
			}

			UpdateVerticalPosition();
		}

		velocity = Vector3.Distance(lastPosition, transform.position) / Time.deltaTime;
		PlayRunningAnimation();
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
		float movement = (IsSlowed) ? movementSpeed / 2 : movementSpeed;
        transform.position += moveInput * movement * Time.deltaTime;
		//charController.Move(moveInput * movement * Time.deltaTime);
	}

	private void UpdateVerticalPosition()
	{

		if (charController.isGrounded)
		{
			verticalVelocity = 0;
		}
		else
		{
			verticalVelocity -= 1;
		}

		verticalMove = new Vector3(0, verticalVelocity, 0);
		charController.Move(verticalMove * Time.deltaTime);
	}

	private void PlayRunningAnimation()
	{
		charAnimator.SetFloat("Velocity", velocity);
	}
}
