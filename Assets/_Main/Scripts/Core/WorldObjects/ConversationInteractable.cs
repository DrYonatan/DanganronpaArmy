using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public abstract class ConversationInteractable : Interactable
{
    public bool isClicked = false;
    public TextAsset text1;
    public TextAsset text2;
    
    protected void FinishInteraction()
    {
        if (WorldManager.instance.currentGameEvent != null)
            StartConversation();
        WorldManager.instance.currentGameEvent.UpdateEvent();
    }

    protected void StartConversation()
    {
        List<string> lines;
        if (!isClicked)
        {
            lines = FileManager.ReadTextAsset(text1);
        }
        else
        {
            lines = FileManager.ReadTextAsset(text2 ? text2 : text1);
        }

        DialogueSystem.instance.Say(lines);
        isClicked = true;
    }
}