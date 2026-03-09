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

    void MovePlayer()
    {
        bool grounded = controller.isGrounded;

        Vector3 inputDirection = Vector3.zero;

        if (playerMovementInput.magnitude > 0.01f)
            inputDirection = transform.TransformDirection(playerMovementInput.normalized);

        if (grounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
            canBoost = true;
            isFastFalling = false;
        }

        // Ground acceleration
        if (!isSliding && grounded)
        {
            float accel = Input.GetKey(KeyCode.LeftShift) ? sprintAcceleration : acceleration;
            horizontalVelocity += inputDirection * accel * Time.deltaTime;
        }

        // Air Strafing
        if (!grounded && !isSliding)
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

        // Clamp horizontal velocity
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);

        // Friction
        float friction = grounded ? groundFriction : 0.02f; // very low air friction
        horizontalVelocity -= horizontalVelocity * friction * Time.deltaTime;

        // Jump
        if ((grounded || (canBoost)) && Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
                canBoost = true;
            else
                canBoost = false;

            verticalVelocity = jumpForce;
            horizontalVelocity *= 1.01f; // preserve momentum
        }

        // Air dash
        if (!grounded && canBoost && Input.GetKeyDown(KeyCode.LeftShift))
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

        // Fast fall
        if (!grounded && Input.GetKeyDown(KeyCode.LeftControl))
            isFastFalling = true;

        if (isFastFalling)
            verticalVelocity -= (boostForce * 4) * Time.deltaTime;

        float gravityMultiplier = verticalVelocity < 0 ? 1.6f : 1f;
        verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;

        Vector3 move = horizontalVelocity + Vector3.up * verticalVelocity;

        controller.Move(move * Time.deltaTime);
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

    void MoveCamera()
    {
        xRotation -= playerMouseInput.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * playerMouseInput.x * sensitivity);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}