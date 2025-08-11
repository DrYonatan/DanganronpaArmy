using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public abstract class ConversationInteractable : Interactable
{
    public bool isClicked = false;
    public List<DialogueNode> text1;
    public List<DialogueNode> text2;
    
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
        List<DialogueNode> nodes = new List<DialogueNode>();
        if (!isClicked)
        {
            nodes = text1;
        }
        else
        {
            nodes = text2;
        }

        DialogueSystem.instance.Say(nodes);
        isClicked = true;
    }
}