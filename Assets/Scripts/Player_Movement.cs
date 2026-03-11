using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Vector3 horizontalVelocity;
    private float verticalVelocity;

    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;

    private float xRotation;

    [Header("Components")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform player;

    [Header("Movement")]
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float sprintAcceleration = 60f;
    [SerializeField] private float groundFriction = 8f;
    [SerializeField] private float maxSpeed = 20f;

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

        HandleSlide();
        MovePlayer();
        MoveCamera();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Only triggers on walls while in air
        if (Mathf.Abs(hit.normal.y) < 0.2f && !controller.isGrounded) 
            HandleWallBounce(hit);
    }

    void MovePlayer()
    {

        Vector3 inputDirection = Vector3.zero;

        if (playerMovementInput.magnitude > 0.01f)
            inputDirection = transform.TransformDirection(playerMovementInput.normalized);

        // Ground reset for special movements 
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
            canBoost = true;
            isFastFalling = false;
        }

        // Ground acceleration
        if (!isSliding && controller.isGrounded)
        {
            float accel = Input.GetKey(KeyCode.LeftShift) ? sprintAcceleration : acceleration;
            horizontalVelocity += inputDirection * accel * Time.deltaTime;
        }

        // Enforce max speed
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);

        // Friction
        float friction = controller.isGrounded ? groundFriction : 0.02f; // very low air friction
        horizontalVelocity -= horizontalVelocity * friction * Time.deltaTime;

        // Handle movements
        HandleJump();
        HandleAirDash();
        HandleAirStrafe();
        HandleFastFall();
        
    }

    void HandleJump()
    {
        if ((controller.isGrounded || (canBoost)) && Input.GetKeyDown(KeyCode.Space))
        {
            if (controller.isGrounded)
                canBoost = true;
            else
                canBoost = false;

            verticalVelocity = jumpForce;
            horizontalVelocity *= 1.01f; // preserve momentum
        }
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

            // slide boost
            horizontalVelocity += slideDirection * 4f;

            player.localScale = new Vector3(1f, 0.5f, 1f);
        }

        if (isSliding && controller.isGrounded)
        {
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
    }

    void HandleAirDash()
    {
        if (!controller.isGrounded && canBoost && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 boostDir = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) boostDir += transform.forward;
            if (Input.GetKey(KeyCode.S)) boostDir -= transform.forward;
            if (Input.GetKey(KeyCode.A)) boostDir -= transform.right;
            if (Input.GetKey(KeyCode.D)) boostDir += transform.right;

            if (boostDir != Vector3.zero)
            {
                horizontalVelocity += boostDir.normalized * boostForce;
                canBoost = false;
            }
        }

    }

    void HandleAirStrafe()
    {
        if (!controller.isGrounded && !isSliding)
        {
            Vector3 horizontalDir = new Vector3(horizontalVelocity.x, 0f, horizontalVelocity.z);
            float speed = horizontalDir.magnitude;

            if (speed > 0.01f)
            {
                // Camera orientation
                Vector3 camForward = playerCamera.forward;
                camForward.y = 0f;
                camForward.Normalize();

                Vector3 camRight = playerCamera.right;
                camRight.y = 0f;
                camRight.Normalize();

                // Input direction relative to camera
                Vector3 inputDir = camForward * Input.GetAxisRaw("Vertical") + camRight * Input.GetAxisRaw("Horizontal");

                if (inputDir != Vector3.zero)
                {
                    inputDir.Normalize();
                    float redirectStrength = 0.02f; // small fraction to redirect current velocity
                    horizontalVelocity = Vector3.Lerp(horizontalDir, inputDir * speed, redirectStrength);
                }
            }
        }
    }

    void HandleFastFall()
    {
        if (!controller.isGrounded && Input.GetKeyDown(KeyCode.LeftControl))
            isFastFalling = true;

        if (isFastFalling)
            verticalVelocity -= (boostForce * 4) * Time.deltaTime;

        float gravityMultiplier = verticalVelocity < 0 ? 1.6f : 1f;
        verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;

        Vector3 move = horizontalVelocity + Vector3.up * verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    void HandleWallBounce(ControllerColliderHit hit)
    {
        // reflect movement angle
        horizontalVelocity = Vector3.Reflect(horizontalVelocity, hit.normal);

        // kick off boost
        horizontalVelocity *= 1.1f;

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