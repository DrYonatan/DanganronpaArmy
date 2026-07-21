using System.Collections.Generic;
using UnityEngine;

public abstract class ConversationInteractable : Interactable
{
    public string id;
    public bool isClicked = false;
    public int clickCount;
    public List<VNConversationSegment> texts = new List<VNConversationSegment>();
    public Transform lookPosition;

    protected override void FinishInteraction()
    {
        float duration = 0.5f;
        CameraManager.instance.initialRotation = Quaternion.Euler(
            0, CameraManager.instance.cameraTransform.rotation.eulerAngles.y,
            CameraManager.instance.cameraTransform.rotation.eulerAngles.z);
        Vector3 targetPosition =
            lookPosition != null
                ? lookPosition.position
                : Vector3.Lerp(CameraManager.instance.cameraTransform.position, transform.position, 0.75f);
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.MoveCameraTo(targetPosition, duration));
        if (lookPosition != null)
        {
            CameraManager.instance.StartCameraCoroutine(
                CameraManager.instance.RotateCameraTo(lookPosition.rotation, duration));
            CameraManager.instance.initialRotation = lookPosition.rotation;
        }

        StartConversation();
    }

    protected void StartConversation()
    {
        VNConversationSegment text = texts[clickCount];
        if (clickCount < texts.Count - 1)
            clickCount++;

        VNNodePlayer.instance.StartConversation(text);
        isClicked = true;
    }
}