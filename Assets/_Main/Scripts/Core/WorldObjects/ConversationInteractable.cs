using System.Collections.Generic;
using UnityEngine;

public abstract class ConversationInteractable : Interactable
{
    public string id;
    public bool isClicked = false;
    public int clickCount;
    public List<VNConversationSegment> texts = new List<VNConversationSegment>();
    
    protected override void FinishInteraction()
    {
        float duration = 0.5f;
        Vector3 targetPosition =
            Vector3.Lerp(CameraManager.instance.cameraTransform.position, transform.position, 0.75f);
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.MoveCameraTo(targetPosition, duration));

        StartConversation();
    }

    private void StartConversation()
    {
        VNConversationSegment text = texts[clickCount];
        if (clickCount < texts.Count - 1)
            clickCount++;

        VNNodePlayer.instance.StartConversation(text);
        isClicked = true;
    }
}