using UnityEngine;

public class First_Person_Movement : MonoBehaviour
{
    private Vector3 verticalVelocity;
    private Vector3 horizontalVelocity;

    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;

    private float xRotation;

    [Header("Components")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform player;

    [Header("Movement")]
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float groundFriction = 8f;
    [SerializeField] private float airFriction = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float sensitivity = 2f;

    [Header("Sprint")]
    [SerializeField] private float sprintAcceleration = 40f;

    [Header("Slide")]
    [SerializeField] private float slideStartSpeed = 14f;
    [SerializeField] private float slideFriction = 20f;
    [SerializeField] private float minSlideSpeed = 2f;
    [SerializeField] private float slideCooldown = 0.8f;
    private bool isSliding;
    private float slideCooldownTimer;
    private float currentSlideSpeed;
    private Vector3 slideDirection;

    void Start() {

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {

        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        HandleSlide();
        MovePlayer();
        MoveCamera();
    }

    private void HandleSlide() {

        slideCooldownTimer -= Time.deltaTime;

        // If Button Pressed & Can Slide
        if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded && playerMovementInput.magnitude > 0.1f && slideCooldownTimer <= 0f && !isSliding) {
            
            isSliding = true;
            slideCooldownTimer = slideCooldown;

            slideDirection = transform.TransformDirection(playerMovementInput.normalized);
            currentSlideSpeed = Mathf.Max(slideStartSpeed, horizontalVelocity.magnitude);

            player.localScale = new Vector3(1f, 0.5f, 1f);
        }

        if (isSliding) {

            currentSlideSpeed -= slideFriction * Time.deltaTime;
            horizontalVelocity = slideDirection * currentSlideSpeed;

            // Slide Cancel
            if (currentSlideSpeed <= minSlideSpeed || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) { // End Slide

                isSliding = false;
                player.localScale = Vector3.one;
            }
        }
    }

    private void MovePlayer() {

        Vector3 inputDirection = transform.TransformDirection(playerMovementInput.normalized);

        // Acceleration
        if (!isSliding && controller.isGrounded) {
            
            // Sprinting
            if(Input.GetKey(KeyCode.LeftShift))
                horizontalVelocity += inputDirection * sprintAcceleration * Time.deltaTime;
            else
                horizontalVelocity += inputDirection * acceleration * Time.deltaTime;
        }

        // Friction
        float friction = controller.isGrounded ? groundFriction : airFriction;
        horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, friction * Time.deltaTime);

        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);

        // Jump
        if (controller.isGrounded) {

            if (verticalVelocity.y < 0)
                verticalVelocity.y = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
                verticalVelocity.y = jumpForce;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        Vector3 finalMove = horizontalVelocity + Vector3.up * verticalVelocity.y;
        
        controller.Move(finalMove * Time.deltaTime);
    }

    private void MoveCamera() {

        xRotation -= playerMouseInput.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * playerMouseInput.x * sensitivity);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}