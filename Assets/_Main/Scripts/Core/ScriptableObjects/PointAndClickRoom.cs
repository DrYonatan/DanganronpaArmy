using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using Cinemachine;

[CreateAssetMenu(menuName="Rooms/Point and Click Room")]
public class PointAndClickRoom : Room
{
    public Room exitRoom;

    [SerializeField] float cameraDollySpeed = 2f;

    public float rotationSpeed = 100f; // Adjust rotation speed
    public float borderRight = 10f;
    public float borderLeft = -10f;
    public float borderUp = -10f;
    public float borderDown = 10f;

    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;

    private float pitch = 0f;

    public override IEnumerator OnLoad()
    {
        base.OnLoad();
        VirutalCameraManager.instance.AssignVirtualCamera();
        return VirutalCameraManager.instance.SlideAcrossRoom(3f, GameObject.Find("World/TrackSlidingPos").transform.position);
    }

    public override void MovementControl()
    {
      float horizontalInput = Input.GetAxis("Horizontal");
      float verticalInput = Input.GetAxis("Vertical");

      float position = VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;
      position += (horizontalInput * cameraDollySpeed * Time.deltaTime);
      VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = position;

      pitch -= verticalInput * rotationSpeed * Time.deltaTime;
      pitch = Mathf.Clamp(pitch, borderUp, borderDown);

      // Apply the updated pitch
      VirutalCameraManager.instance.pitchControl.pitch = pitch;


        if(Input.GetKey(KeyCode.R))
        {
            GameEvent currentGameEvent = WorldManager.instance.currentGameEvent;
            if(currentGameEvent is PointAndClickEvent && !((PointAndClickEvent)(currentGameEvent)).isExitable)
            {
                DialogueSystem.instance.Say(FileManager.ReadTextAsset(WorldManager.instance.currentGameEvent.unallowedText ?
                WorldManager.instance.currentGameEvent.unallowedText : 
                Resources.Load<TextAsset>("GameEvents/unallowed")));
            }
            
            else
            WorldManager.instance.StartLoadingRoom(exitRoom);
        }
        
        ReticleManager.instance.ReticleAsCursor();
      
    }

    public void ResetRotations()
    {
        verticalRotation = 0f;
        horizontalRotation = 0f;
    }
}
