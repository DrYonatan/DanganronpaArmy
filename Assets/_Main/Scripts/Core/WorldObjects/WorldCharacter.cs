using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class WorldCharacter : ConversationInteractable
{
    public override void Interact()
    {
        CharacterClickEffects.instance.Interact(transform);
        base.Interact();
        StartCoroutine(MoveAndRotateCameraTo());
    }

    private IEnumerator MoveAndRotateCameraTo()
    {
        float duration = 0.5f;
        Vector3 targetPosition = transform.position + transform.forward + Vector3.up;
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
        yield return StartCoroutine(CameraManager.instance.RotateCameraTo(
            Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up), duration));
        StartCoroutine(CameraManager.instance.MoveCameraTo(targetPosition, duration));
        StartCoroutine(CameraManager.instance.RotateCameraTo(targetRotation, duration));
        CameraManager.instance.initialRotation = targetRotation;
        FinishInteraction();
    }
}