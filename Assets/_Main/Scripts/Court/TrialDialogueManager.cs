using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TrialDialogueManager : MonoBehaviour, IWorldHandler
{
    public static TrialDialogueManager instance { get; private set; }
    
    [SerializeField] ConversationSegment conversation;
    int conversationIndex;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayConversationNode();
    }

    void PlayConversationNode()
    {
        ConversationNode nextNode = conversation.conversationNodes[conversationIndex];
        List<string> lines = nextNode.textLines;
        DialogueSystem.instance.ShowSpeakerName(nextNode.character.name);
        DialogueSystem.instance.Say(lines);
    }

    void GoToNextNode()
    {
        conversationIndex++;
        PlayConversationNode();
    }

    public void HandleConversationEnd()
    {
        GoToNextNode();
    }

}
