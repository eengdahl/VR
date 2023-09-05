using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2.0f; 
    public Transform playerBody;

    private float rotationX = 0.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the camera horizontally based on mouse X movement
        transform.Rotate(Vector3.up * mouseX * sensitivity);

        // Rotate the player's body vertically based on mouse Y movement
        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);
        playerBody.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
