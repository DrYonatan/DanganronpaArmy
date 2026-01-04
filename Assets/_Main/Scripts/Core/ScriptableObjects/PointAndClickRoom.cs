using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using Cinemachine;

[CreateAssetMenu(menuName="Rooms/Point and Click Room")]
public class PointAndClickRoom : Room
{
    public Room exitRoom;
    public string exitLocation;

    [SerializeField] float cameraDollySpeed = 2f;

    public float rotationSpeed = 100f; // Adjust rotation speed
    public float borderUp = -10f;
    public float borderDown = 10f;

    private float horizontalRotation;
    private float verticalRotation;

    private float pitch;

    public override IEnumerator OnLoad()
    {
        base.OnLoad();
        VirutalCameraManager.instance.AssignVirtualCamera();
        yield return null;
    }

    public override IEnumerator AppearAnimation()
    {
        yield return VirutalCameraManager.instance.SlideAcrossRoom(3f, GameObject.Find("World/TrackSlidingPos").transform.position);
    }

    public override void MovementControl()
    {
      float horizontalInput = Input.GetAxis("Horizontal");
      float verticalInput = Input.GetAxis("Vertical");
      
      if (VirutalCameraManager.instance.virtualCamera == null)
          return;

      float position = VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;
      position += (horizontalInput * cameraDollySpeed * Time.deltaTime);
      VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = position;

      pitch -= verticalInput * rotationSpeed * Time.deltaTime;
      pitch = Mathf.Clamp(pitch, borderUp, borderDown);

      // Apply the updated pitch
      VirutalCameraManager.instance.pitchControl.pitch = pitch;


        if(Input.GetKey(KeyCode.R))
        {
            if(!WorldManager.instance.currentRoomData.isExitable)
            {
                if(ProgressManager.instance.currentGameEvent.unallowedText != null)
                   VNNodePlayer.instance.StartConversation(ProgressManager.instance.currentGameEvent.unallowedText);
            }
            
            else
            {
                WorldManager.instance.StartLoadingRoom(exitRoom, exitLocation);
            }
        }
        
        CursorManager.instance.ReticleAsCursor();
      
    }

    public void ResetRotations()
    {
        verticalRotation = 0f;
        horizontalRotation = 0f;
    }

    public override void OnConversationEnd()
    {
        CameraManager.instance.ReturnToDollyTrack();
    }
}
