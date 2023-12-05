using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterController : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public Transform _cam;
    public float verticalLookRange = 80f; // Set your desired vertical look range in degrees

    private float rotationX = 0f;

    private void Update()
    {
        // Get input from the player
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Move the player
        Vector3 movement = new Vector3(horizontalMovement, 0, verticalMovement) * speed * Time.deltaTime;
        transform.Translate(movement);

        // Rotate the player based on mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the player around the Y-axis
        transform.Rotate(Vector3.up * mouseX * sensitivity);

        // Rotate the camera around the X-axis (vertical rotation)
        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -verticalLookRange, verticalLookRange);

        _cam.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
