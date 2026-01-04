using System.Collections;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;

[CreateAssetMenu(menuName = "Rooms/Free Roam Room")]
public class FreeRoamRoom : Room
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;
    public float maxLookAngle = 80f; // Prevents camera flipping

    float playerReach = 14f;

    public Sprite map;

    private float bobTimer;

    public override void MovementControl()
    {
        MapContainer.instance.HandleMapVisibility();
        Move();
        Look();
        Interact();
    }

    void Move()
    {
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= 2;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = CameraManager.instance.cameraTransform.right * horizontal + CameraManager.instance.cameraTransform.forward * vertical;
        move.y = 0; // Ensure no vertical movement
        CharacterController controller = CameraManager.instance.player;
        controller.Move(move * Time.deltaTime * speed);
        CameraHeadBobbing();
    }

    void Look()
    {
        CenterCursor();

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        horizontalRotation += mouseX;

        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        CameraManager.instance.cameraTransform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }

    void Interact()
    {
        CheckInteraction();
        if (Input.GetMouseButtonDown(0) && currentInteractable != null 
                                        && !DialogueSystem.instance.isActive)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (newInteractable.enabled)
                {
                    currentInteractable = newInteractable;
                }
                else
                {
                    if(currentInteractable != null)
                    {
                        currentInteractable.OnStopLooking();
                        currentInteractable = null;
                    }
                    
                }
            }
            else
            {
                if(currentInteractable != null)
                {
                    currentInteractable.OnStopLooking();
                    currentInteractable = null;
                }
            }
        }
        else
        {
            if(currentInteractable != null)
            {
                currentInteractable.OnStopLooking();
                currentInteractable = null;
            }
        }
    }

    void CenterCursor()
    {
        RectTransform cursor = CursorManager.instance.cursor;
        cursor.anchorMin = new Vector2(0.5f, 0.5f);
        cursor.anchorMax = new Vector2(0.5f, 0.5f);
        cursor.pivot = new Vector2(0.5f, 0.5f);

        // Set position to center
        cursor.anchoredPosition = Vector2.zero;
    }

    public override IEnumerator OnLoad()
    {
        base.OnLoad();
        CameraManager.instance.cameraTransform.localPosition = Vector3.zero;
        MapContainer.instance.SetMap(map);
        yield return null;
    }

    public override void OnConversationEnd()
    {
        CameraManager.instance.cameraTransform.DOLocalMove(Vector3.zero, 0.5f);
    }

    private void CameraHeadBobbing()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        float speed = 1f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 1.5f;
        }

        bool isMoving = new Vector2(horizontal, vertical).magnitude > 0.1f;

        Vector3 targetLocalPos = Vector3.zero;

        if (isMoving)
        {
            bobTimer += Time.deltaTime * 15f * speed;
            float bobOffset = Mathf.Sin(bobTimer) * 0.2f;
            targetLocalPos = new Vector3(0f, bobOffset, 0f);
        }
        else
        {
            bobTimer = 0f;
        }

        // Smooth transition (prevents snapping)
        CameraManager.instance.cameraTransform.localPosition = Vector3.Lerp(
            CameraManager.instance.cameraTransform.localPosition,
            targetLocalPos,
            Time.deltaTime * 10f
        );
    }
}