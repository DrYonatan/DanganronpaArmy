using System.Collections;
using UnityEngine;

public class WorldObject : ConversationInteractable
{
    public override void Interact()
    {
        base.Interact();
        StartCoroutine(MoveAndRotateCameraTo());
    }

    private IEnumerator MoveAndRotateCameraTo()
    {
        float duration = 0.5f;
        Vector3 targetPosition = Vector3.Lerp(Camera.main.transform.position, transform.position, 0.75f);
        Quaternion targetRotation =
            Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);

        yield return CameraManager.instance.StartCameraCoroutine(CameraManager.instance.RotateCameraTo(targetRotation, duration));
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.MoveCameraTo(targetPosition, duration));
        FinishInteraction();
    }
}