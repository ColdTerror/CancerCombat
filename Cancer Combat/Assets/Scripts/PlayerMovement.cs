using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public float speed = 0;
    public float mouseSensitivity = 50f; // Adjust this for camera sensitivity
    public Transform cameraTransform; // Drag your FirstPersonCamera here
    public float verticalRotationLimit = 80f; // Limit how far up/down you can look

    private float xRotation = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        rb = GetComponent<Rigidbody>();

        // Ensure the camera transform is assigned
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned in the Inspector!");
            enabled = false; // Disable the script if the camera is missing
            return;
        }

        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnLook(InputValue lookValue)
    {
        Vector2 lookVector = lookValue.Get<Vector2>();
        float mouseX = lookVector.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookVector.y * mouseSensitivity * Time.deltaTime;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalRotationLimit, verticalRotationLimit);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void FixedUpdate()
    {
        // Get the input movement vector
        Vector3 movementInput = new Vector3(movementX, 0.0f, movementY).normalized;

        // Get the camera's forward and right vectors (ignoring vertical component)
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the desired movement direction in world space
        Vector3 desiredMovementDirection = (cameraForward * movementInput.z) + (cameraRight * movementInput.x);

        // Apply the force
        rb.AddForce(desiredMovementDirection * speed);
    }

    private void Update()
    {
        // Unlock cursor if Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


}