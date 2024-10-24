using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;        // Player movement speed
    [SerializeField] private float rotationSpeed = 100f;  // Speed of rotation
    [SerializeField] private float mouseSensitivity = 50f; // Mouse sensitivity for rotation
    [SerializeField] private float initialRotationOffset = -90f; // Initial rotation offset to compensate for

    private Vector3 moveDirection;
    private float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the cursor and lock it in the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set the initial rotation based on the starting offset (e.g., -90 degrees)
        rotationY = initialRotationOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle player movement input
        HandleMovement();

        // Handle player rotation using the mouse
        HandleRotation();
    }

    // Method to handle player movement relative to the player's rotation
    void HandleMovement()
    {
        // Get input from the keyboard (W, A, S, D or Arrow Keys)
        float moveX = -(Input.GetAxis("Vertical"));  // Left/Right (A/D or Left Arrow/Right Arrow)
        float moveZ = Input.GetAxis("Horizontal");    // Forward/Backward (W/S or Up Arrow/Down Arrow)

        // Calculate the movement direction based on input and rotation
        moveDirection = transform.right * moveX + transform.forward * moveZ;  // Relative to the player's rotation

        // Move the player relative to the player's local space
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    // Method to handle player rotation using mouse
    void HandleRotation()
    {
        // Get the mouse movement along the X-axis
        float mouseX = Input.GetAxis("Mouse X");

        // Rotate the parent object (the object this script is attached to) along the Y-axis
        rotationY += mouseX * rotationSpeed * mouseSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
