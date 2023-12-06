using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterController : MonoBehaviour
{
    public float speed = 5f;  // Adjust this value to set the movement speed.
    public float sensitivity = 2f;  // Adjust this value to set the camera sensitivity.
    public Transform playerCamera;

    private Rigidbody rb;
    private float rotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;  // Locks the cursor to the center of the screen.
    }

    void Update()
    {
        // Player movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        rb.AddForce(transform.TransformDirection(movement) * speed);

        // Player camera rotation
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate the player around the Y-axis (horizontal movement)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera around the X-axis (vertical movement), with clamping to prevent over-rotation
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}
