using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public float mouseSensitivity = 0.1f;

    // Jumping and falling vars
    public float gravity = -9.81f;
    public float jumpHeight = .1f;

    // Jump check
    private bool isGrounded;
    public float standingHeight = 2f;
    public Transform cameraTransform;

    // Crouch vars
    public float crouchHeight = 1f;
    public float crouchSpeedMult = 0.5f;
    private bool isCrouching = false;
    private bool crouchHeld = false;
    private Vector3 standingCenter;
    private Vector3 crouchCenter;
    private float standingCamHeight;
    private float crouchCamHeight;

    // Sprint vars
    public float sprintSpeedMultiplier = 1.5f;
    private bool isSprinting = false;

    // Sprint FOV vars
    public float sprintFOVMultiplier = 1.1f;
    private float baseFOV;
    private Camera playerCamera;

    private CharacterController controller;
    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private float yVelocity;

    void Awake()
    {
        controls = new PlayerControls();

        // Move and look value reads
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // Jump value read
        controls.Player.Jump.performed += ctx => Jump();

        // Crouch value reads
        controls.Player.Crouch.performed += ctx => {
            crouchHeld = true;
            startCrouch();
        };

        controls.Player.Crouch.canceled += ctx => {
            crouchHeld = false;
            stopCrouch();
        };

        // Sprint value reads
        controls.Player.Sprint.performed += ctx => isSprinting = true;
        controls.Player.Sprint.canceled += ctx => isSprinting = false;
    }

    void OnEnable()
    {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Standing height and center
        standingHeight = controller.height;
        standingCenter = controller.center;

        // Crouch center - sits lower so the base doesnt lift off ground
        crouchCenter = new Vector3(0, standingCenter.y - (standingHeight - crouchHeight) / 2f, 0);

        // Camera heights - store camera heights relative to the character root
        standingCamHeight = cameraTransform.localPosition.y;
        crouchCamHeight = standingCamHeight - (standingHeight - crouchHeight);

        // Sprint FOV
        playerCamera = cameraTransform.GetComponent<Camera>();
        baseFOV = playerCamera.fieldOfView;
    }

    void Update()
    {
        // Grounded check
        isGrounded = controller.isGrounded;
        if (isGrounded && yVelocity < 0)  // If grounded and being pulled by gravity
        {
            yVelocity = -2f; // Stick to ground and remove stacking gravity
        }

        // Gravity
        yVelocity += gravity * Time.deltaTime;

        // Mouse look and camera rotation
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movement and speed
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float baseSpeed = speed;
        if (isCrouching) 
        {
        baseSpeed *= crouchSpeedMult; // Slow player if crouching
        }

        if (isSprinting && !isCrouching && isGrounded) 
        {
            baseSpeed *= sprintSpeedMultiplier; // Start sprinting if grounded and not crouching
        }

        Vector3 velocity = move * baseSpeed + Vector3.up * yVelocity;
        controller.Move(velocity * Time.deltaTime);

        // Sprinting increases FOV
        float targetFOV = isSprinting && !isCrouching ? baseFOV * sprintFOVMultiplier : baseFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * 10f);

        // If the player hits their head cancel upward momentum.
        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && yVelocity > 0)
        {
            yVelocity = 0f;
        }

        // Auto stand when possible
        if (!crouchHeld && isCrouching && canStand())
        {
            standUp();
        }

        // Smooth camera height transition
        float targetCamY = isCrouching ? crouchCamHeight : standingCamHeight;
        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCamY, Time.deltaTime * 10f);
        cameraTransform.localPosition = camPos;
    }

    // Start crouch
    void startCrouch()
    {
        // Dont crouch if jumping
        if (!isGrounded || yVelocity > 0f) 
        {
            return;
        }

        isCrouching = true;
        controller.height = crouchHeight;
        controller.center = crouchCenter;
    }

    // Stop crouch
    void stopCrouch()
    {
        if (!canStand()) 
        {
            return;
        }

        standUp();
    }

    // Auto stand
    void standUp()
    {
        isCrouching = false;
        controller.height = standingHeight;
        controller.center = standingCenter;
    }

    // Jump
    void Jump()
    {
        // Dont jump if aerial or crouching
        if (isGrounded && canStand() && isCrouching == false) 
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -1f * gravity);
            isGrounded = false;
        }
    }

    // Can player stand check
    bool canStand()
    {
        float radius = controller.radius;
        Vector3 bottom = transform.position - Vector3.up * (controller.height / 2 - controller.radius);
        Vector3 top = transform.position + Vector3.up * (controller.height / 2 - controller.radius);
        return !Physics.CheckCapsule(bottom, top, radius);
    }
}