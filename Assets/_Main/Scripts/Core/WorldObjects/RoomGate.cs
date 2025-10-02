using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Linq;


public class RoomGate : Interactable
{
    public Room roomToLoad;
    public string entryPoint;

    public override void FinishInteraction()
    {
         if (((FreeRoamEvent)(WorldManager.instance.currentGameEvent)).allowedRooms.Any(item =>
                item.name == roomToLoad.name) ||
            ((FreeRoamEvent)(WorldManager.instance.currentGameEvent)).allowedRooms.Count == 0)
        {
            StartCoroutine(RoomTransition());
        }
        else
        {
            // if the room is unallowed, read the "you can't go into this room" text
            VNNodePlayer.instance.StartConversation(WorldManager.instance.currentGameEvent.unallowedText);
        }
    }

    IEnumerator RoomTransition()
    {
        WorldManager.instance.isLoading = true;
        Vector3 targetPosition = new Vector3(transform.position.x + transform.forward.x * 9f, Camera.main.transform.position.y, transform.position.z + transform.forward.z * 9f);
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.RotateCameraTo(Quaternion.LookRotation(transform.forward * -1), 0.5f));
        yield return CameraManager.instance.MoveCameraTo(targetPosition, 0.5f);
        ImageScript.instance.FadeToBlack(1f);
        yield return CameraManager.instance.MoveCameraTo(targetPosition + -5 * transform.forward, 1.5f);
        WorldManager.instance.StartLoadingRoom(roomToLoad, entryPoint);
    }
}