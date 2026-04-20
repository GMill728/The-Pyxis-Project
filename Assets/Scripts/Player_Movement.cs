using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Vector3 horizontalVelocity;
    private float verticalVelocity;

    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;

    private float xRotation;

    private bool isGrounded;

    [Header("Components")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform player;

    [Header("Movement")]
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float sprintAcceleration = 60f;
    [SerializeField] private float groundFriction = 8f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float absoluteMaxSpeed = 100f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float gravity = -20f;

    [Header("Boost")]
    [SerializeField] private float boostForce = 12f;
    private bool canBoost = true;

    [Header("Slide")]
    [SerializeField] private float slideStartSpeed = 14f;
    [SerializeField] private float slideFriction = 20f;
    [SerializeField] private float minSlideSpeed = 2f;
    [SerializeField] private float slideCooldown = 0.8f;

    private bool isSliding;
    private float slideCooldownTimer;
    private float currentSlideSpeed;
    private Vector3 slideDirection;

    [Header("Fast Fall")]
    private bool isFastFalling;

    [Header("Mouse")]
    [SerializeField] private float sensitivity = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        playerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        isGrounded = controller.isGrounded;

        MovePlayer();
        MoveCamera();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Mathf.Abs(hit.normal.y) < 0.2f && !isGrounded)
            HandleWallBounce(hit);
    }

    void MovePlayer()
    {
        Vector3 inputDirection = GetInputDirection();

        HandleGroundReset();
        HandleAcceleration(inputDirection);
        ApplyFriction();

        HandleJump();
        HandleAirDash();
        HandleAirStrafe();
        HandleFastFall();
        HandleSlide();

        ApplyMovement();
    }

    Vector3 GetInputDirection()
    {
        if (playerMovementInput.magnitude < 0.01f)
            return Vector3.zero;

        return transform.TransformDirection(playerMovementInput.normalized);
    }

    void HandleGroundReset()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
            canBoost = true;
            isFastFalling = false;
        }
    }

    void HandleAcceleration(Vector3 inputDirection)
    {
        if (isSliding || !isGrounded) return;

        float accel = Input.GetKey(KeyCode.LeftShift) ? sprintAcceleration : acceleration;
        horizontalVelocity += inputDirection * accel * Time.deltaTime;

        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);
    }

    void ApplyFriction()
    {
        float friction = isGrounded ? groundFriction : 0.02f;
        horizontalVelocity -= horizontalVelocity * friction * Time.deltaTime;
    }

    void HandleJump()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (isGrounded)
            canBoost = true;
        else if (!canBoost)
            return;
        else
            canBoost = false;

        verticalVelocity = jumpForce;
        horizontalVelocity *= 1.01f;
    }

    void HandleSlide()
    {
        slideCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftControl) &&
            playerMovementInput.magnitude > 0.1f &&
            slideCooldownTimer <= 0 &&
            !isSliding)
        {
            isSliding = true;
            slideCooldownTimer = slideCooldown;

            slideDirection = transform.TransformDirection(playerMovementInput.normalized);
            currentSlideSpeed = Mathf.Max(slideStartSpeed, horizontalVelocity.magnitude);

            horizontalVelocity += slideDirection * 4f;
            player.localScale = new Vector3(1f, 0.5f, 1f);
        }

        if (!isSliding || !isGrounded) return;

        currentSlideSpeed -= slideFriction * Time.deltaTime;

        horizontalVelocity = Vector3.Lerp(
            horizontalVelocity,
            slideDirection * currentSlideSpeed,
            10f * Time.deltaTime
        );

        if (currentSlideSpeed <= minSlideSpeed ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSliding = false;
            player.localScale = Vector3.one;
        }
    }

    void HandleAirDash()
    {
        if (isGrounded || !canBoost || !Input.GetKeyDown(KeyCode.LeftShift))
            return;

        Vector3 boostDir =
            transform.forward * playerMovementInput.z +
            transform.right * playerMovementInput.x;

        if (boostDir == Vector3.zero) return;

        horizontalVelocity += boostDir.normalized * boostForce;
        canBoost = false;
    }

    void HandleAirStrafe()
    {
        if (isGrounded || isSliding) return;

        Vector3 horizontalDir = new Vector3(horizontalVelocity.x, 0f, horizontalVelocity.z);
        float speed = horizontalDir.magnitude;

        if (speed < 0.01f) return;

        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 inputDir = camForward * playerMovementInput.z + camRight * playerMovementInput.x;

        if (inputDir == Vector3.zero) return;

        inputDir.Normalize();
        horizontalVelocity = Vector3.Lerp(horizontalDir, inputDir * speed, 0.02f);
    }

    void HandleFastFall()
    {
        if (!isGrounded && Input.GetKeyDown(KeyCode.LeftControl))
            isFastFalling = true;

        if (isFastFalling)
            verticalVelocity -= (boostForce * 4f) * Time.deltaTime;

        float gravityMultiplier = verticalVelocity < 0 ? 1.6f : 1f;
        verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;
    }

    void ApplyMovement()
    {
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, absoluteMaxSpeed);
        Vector3 move = horizontalVelocity + Vector3.up * verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    void HandleWallBounce(ControllerColliderHit hit)
    {
        horizontalVelocity = Vector3.Reflect(horizontalVelocity, hit.normal) * 0.5f; //was originally .9f
        canBoost = true;
    }

    void MoveCamera()
    {
        xRotation -= playerMouseInput.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * playerMouseInput.x * sensitivity);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}