using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Rooms/Point and Click Room")]
public class PointAndClickRoom : Room
{
    public float rotationSpeed = 100f; // Adjust rotation speed
    public float borderRight = 10f;
    public float borderLeft = -10f;
    public float borderUp = -10f;
    public float borderDown = 10f;

    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;

    public override void MovementControl()
    {
     float horizontalInput = 0f;
     float verticalInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f; // Rotate left
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f; // Rotate right
        }
        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = -1f; // Rotate up
        }
        if (Input.GetKey(KeyCode.S))
        {
            verticalInput = 1f; // Rotate down
        }

      

        horizontalRotation += horizontalInput * rotationSpeed * Time.deltaTime;
        verticalRotation += verticalInput * rotationSpeed * Time.deltaTime;
        // Rotate the camera around the Y-axis

        horizontalRotation = Mathf.Clamp(horizontalRotation, borderLeft, borderRight);
        verticalRotation = Mathf.Clamp(verticalRotation, borderUp, borderDown);
      
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }

    public void ResetRotations()
    {
        verticalRotation = 0f;
        horizontalRotation = 0f;
    }
}
