using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TrialDialogueManager : MonoBehaviour, IWorldHandler
{
    public static TrialDialogueManager instance { get; private set; }
    
    [SerializeField] ConversationSegment conversation;
    int conversationIndex;
    [SerializeField] CameraEffectController effectController;
    [SerializeField] CameraController cameraController;
    [SerializeField] List<CharacterStand> characterStands;

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
        CharacterStand characterStand = characterStands.Find(stand => stand.character == nextNode.character);
        cameraController.TeleportToTarget(characterStand.transform);
        List<string> lines = nextNode.textLines;
        DialogueSystem.instance.ShowSpeakerName(nextNode.character.name);
        DialogueSystem.instance.Say(lines);
        effectController.StartEffect(nextNode.cameraEffect);
    }

    void GoToNextNode()
    {
        conversationIndex++;
        effectController.Reset();
        PlayConversationNode();
    }

    public void HandleConversationEnd()
    {
        GoToNextNode();
    }

}
