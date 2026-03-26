using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f;
    public float mouseSensitivity = 0.1f;

    public Transform cameraTransform;

    private CharacterController controller;
    private PlayerControls controls;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float xRotation = 0f;

    void Awake() {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable() {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable() {
        controls.Disable();
    }

    void Start() {
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // player look vector debug line
        Vector3 cameraPosition = cameraTransform.position + cameraTransform.localPosition;
        Debug.DrawLine(cameraPosition, cameraPosition + (cameraTransform.forward * 2), Color.blue);

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);
    }
}