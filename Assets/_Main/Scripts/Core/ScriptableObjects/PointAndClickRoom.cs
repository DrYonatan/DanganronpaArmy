using System.Collections;
using System.Linq;
using UnityEngine;
using DIALOGUE;
using Cinemachine;

[CreateAssetMenu(menuName = "Rooms/Point and Click Room")]
public class PointAndClickRoom : Room
{
    public Room exitRoom;
    public string exitLocation;

    [SerializeField] float cameraDollySpeed = 2f;
    public float dollyDuration = 3f;

    public float rotationSpeed = 100f; // Adjust rotation speed
    public float borderUp = -10f;
    public float borderDown = 10f;

    private float horizontalRotation;
    private float verticalRotation;

    private float pitch;

    public bool hasStartAnimation = true;

    public override IEnumerator OnLoad()
    {
        VirutalCameraManager.instance
            .AssignVirtualCamera(); // Important, this must happen before the call to base, because base.OnLoad() yield returns null (meaning it waits for one frame), and this is problematic when loading a save, the extra frame before the vCam is disabled causes the master camera's position to change
        yield return base.OnLoad();
    }

    public override IEnumerator AppearAnimation()
    {
        if (hasStartAnimation)
        {
            yield return VirutalCameraManager.instance.SlideAcrossRoom(dollyDuration,
                GameObject.Find("World/TrackSlidingPos").transform.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void MovementControl()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (VirutalCameraManager.instance.virtualCamera == null)
            return;

        float position = VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>()
            .m_PathPosition;
        position += (horizontalInput * cameraDollySpeed * Time.deltaTime);
        position = Mathf.Clamp(position, 0,
            VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path
                .PathLength - 1);
        VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition =
            position;

        pitch -= verticalInput * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, borderUp, borderDown);

        // Apply the updated pitch
        VirutalCameraManager.instance.pitchControl.pitch = pitch;


        if (Input.GetKey(KeyCode.R))
        {
            if (!WorldManager.instance.currentRoomData.isExitable || ProgressManager.instance.currentGameEvent.roomDatas.All(item =>
                    item.room.roomName != exitRoom.roomName))
            {
                WorldEvent currentEvent = ProgressManager.instance.currentGameEvent as WorldEvent;
                if (currentEvent == null)
                    return;

                VNConversationSegment unallowed = currentEvent.unallowedText
                    ? currentEvent.unallowedText
                    : WorldManager.instance.unallowedRoomText;
                
                VNNodePlayer.instance.StartConversation(unallowed);
            }
            else
            {
                WorldManager.instance.StartLoadingRoom(exitRoom, exitLocation);
            }
        }

        CursorManager.instance.ReticleAsCursor();
        if (Input.GetMouseButtonDown(0) && !DialogueSystem.instance.isActive &&
            WorldManager.instance.currentRoom.currentInteractable)
        {
            WorldManager.instance.currentRoom.currentInteractable.Interact();
            VirutalCameraManager.instance.virtualCamera.gameObject.SetActive(false);
        }
    }

    public override void OnConversationEnd()
    {
        CameraManager.instance.ReturnToDollyTrack();
    }
}