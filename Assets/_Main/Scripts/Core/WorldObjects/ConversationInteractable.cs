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
        if (WorldManager.instance.currentGameEvent != null)
            StartConversation();
        WorldManager.instance.currentGameEvent.UpdateEvent();
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

        VNDialogueManager.instance.StartConversation(text);
        isClicked = true;
    }
}