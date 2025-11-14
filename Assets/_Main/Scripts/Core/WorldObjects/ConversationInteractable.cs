using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public abstract class ConversationInteractable : Interactable
{
    public bool isClicked = false;
    public VNConversationSegment text1;
    public VNConversationSegment text2;
    
    public override void FinishInteraction()
    {
        float duration = 0.5f;
        Vector3 targetPosition = Vector3.Lerp(Camera.main.transform.position, transform.position, 0.75f);
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.MoveCameraTo(targetPosition, duration));
        if (ProgressManager.instance.currentGameEvent != null)
            StartConversation();
        ProgressManager.instance.currentGameEvent.CheckIfFinished();
    }

    protected void StartConversation()
    {
        VNConversationSegment text = ScriptableObject.CreateInstance<VNConversationSegment>();
        if (!isClicked)
        {
            text = text1;
        }
        else
        {
            text = text2;
        }

        VNNodePlayer.instance.StartConversation(text);
        isClicked = true;
    }
}