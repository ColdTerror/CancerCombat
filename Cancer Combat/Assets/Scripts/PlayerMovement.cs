using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public float speed = 1.2f; // Slower walking speed for natural strides
    public float acceleration = 4f; // Smooth acceleration
    public float deceleration = 4f; // Smooth deceleration
    private Vector3 currentVelocity = Vector3.zero;

    public float mouseSensitivity = 10f; // Adjust this for camera sensitivity
    public Transform cameraTransform; // Drag your FirstPersonCamera here
    public float verticalRotationLimit = 80f; // Limit how far up/down you can look

    private float xRotation = 0f;

    // Head bobbing variables
    public float bobFrequency = 1.8f; // Frequency of the bobbing (steps per second)
    public float bobAmplitudeVertical = 0.15f; // Vertical amplitude of the bobbing
    public float bobAmplitudeHorizontal = 0.05f; // Horizontal amplitude of the bobbing
    private float bobTimer = 0.0f;
    private Vector3 initialCameraPosition;

    // Footstep sound variables
    public AudioSource footstepAudioSource; // Assign an AudioSource for footsteps
    public AudioClip[] footstepSounds; // Array of footstep sounds
    private float footstepTimer = 0.0f;
    private float footstepInterval; // Automatically calculated based on bobFrequency

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

        // Store the initial position of the camera for head bobbing
        initialCameraPosition = cameraTransform.localPosition;

        // Calculate footstep interval based on bob frequency
        footstepInterval = 1.0f / bobFrequency;

        // Ensure the AudioSource is assigned
        if (footstepAudioSource == null)
        {
            Debug.LogError("Footstep AudioSource not assigned in the Inspector!");
        }
    }

    void OnMove(InputValue movementValue)
    {
        if (enabled)
        {
            // Get the input movement vector
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
        }
    }

    void OnLook(InputValue lookValue)
    {
        if (enabled)
        {
            // Get the mouse movement input
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
    }

    private void FixedUpdate()
    {
        // Get the input movement vector
        Vector3 movementInput = new Vector3(movementX, 0.0f, movementY).normalized;

        // Smoothly accelerate or decelerate the player's movement
        Vector3 targetVelocity = movementInput * speed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, (movementInput.magnitude > 0 ? acceleration : deceleration) * Time.fixedDeltaTime);

        // Apply the velocity to the Rigidbody
        rb.linearVelocity = transform.TransformDirection(currentVelocity);
    }

    private void Update()
    {
        // Unlock cursor if Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Time.timeScale == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Handle head bobbing
        HandleHeadBobbing();

        // Handle footstep sounds
        HandleFootsteps();
    }

    private void HandleHeadBobbing()
    {
        // Calculate the magnitude of movement
        float movementMagnitude = currentVelocity.magnitude;

        if (movementMagnitude > 0.1f) // Only bob when moving
        {
            bobTimer += Time.deltaTime * bobFrequency;

            // Calculate vertical and horizontal bobbing offsets
            float verticalOffset = Mathf.Sin(bobTimer) * bobAmplitudeVertical;
            float horizontalOffset = Mathf.Cos(bobTimer * 2) * bobAmplitudeHorizontal;

            // Apply the bobbing offsets to the camera
            cameraTransform.localPosition = initialCameraPosition + new Vector3(horizontalOffset, verticalOffset, 0);
        }
        else
        {
            // Reset the bobbing when not moving
            bobTimer = 0.0f;
            cameraTransform.localPosition = initialCameraPosition;
        }
    }

    private void HandleFootsteps()
    {
        // Calculate the magnitude of movement
        float movementMagnitude = currentVelocity.magnitude;

        if (movementMagnitude > 0.1f) // Only play footsteps when moving
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval)
            {
                footstepTimer = 0.0f;

                // Play a random footstep sound
                if (footstepSounds.Length > 0 && footstepAudioSource != null)
                {
                    int randomIndex = Random.Range(0, footstepSounds.Length);
                    footstepAudioSource.PlayOneShot(footstepSounds[randomIndex]);
                }
            }
        }
        else
        {
            // Reset the footstep timer when not moving
            footstepTimer = 0.0f;
        }
    }
}