using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Rooms/Free Roam Room")]
public class FreeRoamRoom : Room
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;
    public float maxLookAngle = 80f; // Prevents camera flipping

    public override void MovementControl() 
    {
        Move();
        Look();
    }
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal"); // A (-1) / D (1)
        float moveZ = Input.GetAxis("Vertical");   // W (1) / S (-1)

        Vector3 move = Camera.main.transform.right * moveX + Camera.main.transform.forward * moveZ;
        Camera.main.transform.position += new Vector3(move.x, 0, move.z) * moveSpeed * Time.deltaTime;
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        horizontalRotation += mouseX;

        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }
}
