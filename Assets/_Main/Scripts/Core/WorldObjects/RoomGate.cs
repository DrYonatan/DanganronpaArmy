using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Linq;


public class RoomGate : Interactable
{
    public Room roomToLoad;
    public string entryPoint;

    protected override void FinishInteraction()
    {
        if (ProgressManager.instance.currentGameEvent.roomDatas.Any(item =>
                item.room.roomName == roomToLoad.roomName) ||
            ProgressManager.instance.currentGameEvent.roomDatas.Count == 0)
        {
            PlayerInputManager.instance.DisableInput();
            StartCoroutine(RoomTransition());
        }
        else
        {
            // if the room is unallowed, read the "you can't go into this room" text
            VNNodePlayer.instance.StartConversation(((WorldEvent)ProgressManager.instance.currentGameEvent)
                .unallowedText);
        }
    }

    IEnumerator RoomTransition()
    {
        CameraManager.instance.footStepsSource.Stop();
        WorldManager.instance.isLoading = true;
        Vector3 targetPosition = new Vector3(transform.position.x + transform.forward.x * 9f,
            Camera.main.transform.position.y, transform.position.z + transform.forward.z * 9f);
        CameraManager.instance.StartCameraCoroutine(
            CameraManager.instance.RotateCameraTo(Quaternion.LookRotation(transform.forward * -1), 0.5f));
        yield return CameraManager.instance.MoveCameraTo(targetPosition, 0.5f);
        ImageScript.instance.FadeToBlack(1f);
        yield return CameraManager.instance.MoveCameraTo(targetPosition + -5 * transform.forward, 1.5f);

        RoomData data =
            ProgressManager.instance.currentGameEvent.roomDatas.Find(roomData =>
                roomData.room.roomName.Equals(roomToLoad.roomName));

        if (data.preLoadText != null)
        {
            ImageScript.instance.ShowBackground(Resources.Load<Sprite>("Images/black-bg"), 0f);
            ImageScript.instance.UnFadeToBlack(0.5f);
            yield return VNNodePlayer.instance.StartConversationPipeline(data.preLoadText);
        }

        WorldManager.instance.StartLoadingRoom(roomToLoad, entryPoint);
    }
}