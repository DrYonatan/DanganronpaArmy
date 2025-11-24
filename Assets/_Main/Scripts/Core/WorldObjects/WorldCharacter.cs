using System.Collections;
using UnityEngine;

public class WorldCharacter : ConversationInteractable
{
    public string characterName;

    protected override void FinishInteraction()
    {
        CharacterClickEffects.instance.Interact(transform);
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.RotateCameraTo(targetRotation, 0.5f));
        CameraManager.instance.initialRotation = transform.rotation;
        if (ProgressManager.instance.currentGameEvent != null)
        {
            ProgressManager.instance.currentGameEvent.charactersData[name] = new ObjectData(true);
        }
        base.FinishInteraction();
    }

    public override void OnLook()
    {
        if (!isAlreadyLooking)
        {
            isAlreadyLooking = true;
            CursorManager.instance.ShowOrHideConversationIcon(true);
            CursorManager.instance.ShowOrHideInteractableName(true, characterName);
        }
    }

    public override void OnStopLooking()
    {
        isAlreadyLooking = false;
        CursorManager.instance.ShowOrHideConversationIcon(false);
        CursorManager.instance.ShowOrHideInteractableName(false, "");
    }

    private IEnumerator MoveAndRotateCameraTo()
    {
        float duration = 0.5f;
        Vector3 targetPosition = transform.position + transform.forward + Vector3.up;
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
        yield return CameraManager.instance.StartCameraCoroutine(CameraManager.instance.RotateCameraTo(
            Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up), duration));
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.MoveCameraTo(targetPosition, duration));
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.RotateCameraTo(targetRotation, duration));
        CameraManager.instance.initialRotation = targetRotation;
        FinishInteraction();
    }
}