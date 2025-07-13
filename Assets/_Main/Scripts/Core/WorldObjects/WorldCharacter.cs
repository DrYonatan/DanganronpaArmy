using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class WorldCharacter : ConversationInteractable
{
    public string name;

    public override void Interact()
    {
        CharacterClickEffects.instance.Interact(transform);
        base.Interact();
        StartCoroutine(MoveAndRotateCameraTo());
    }

    public override void OnLook()
    {
        if(!isAlreadyLooking)
        {
            isAlreadyLooking = true;
            ReticleManager.instance.ShowOrHideConversationIcon(true);
            ReticleManager.instance.ShowOrHideInteractableName(true, name);
        }
        
    }

    public override void OnStopLooking()
    {
        isAlreadyLooking = false;
        ReticleManager.instance.ShowOrHideConversationIcon(false);
        ReticleManager.instance.ShowOrHideInteractableName(false, "");
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