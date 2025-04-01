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
        float speed = moveSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed *= 2;
        }

        GameObject gameObject = Camera.main.transform.gameObject;
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = gameObject.transform.right * horizontal + gameObject.transform.forward * vertical;
        move.y = 0; // Ensure no vertical movement
        CharacterController controller = gameObject.GetComponent<CharacterController>();
        controller.Move(move * Time.deltaTime * speed);
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
