using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TrialDialogueManager : MonoBehaviour
{
    public static TrialDialogueManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] ConversationSegment conversation;
    int conversationIndex;

    void PlayConversationNode()
    {
        ConversationNode nextNode = conversation.conversationNodes[conversationIndex];
        List<string> lines = nextNode.textLines;
        DialogueSystem.instance.Say(lines);
    }

    void GoToNextNode()
    {
        conversationIndex++;
        PlayConversationNode();
    }

}
