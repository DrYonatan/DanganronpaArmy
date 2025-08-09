using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TrialDialogueManager : MonoBehaviour, IWorldHandler
{
    public static TrialDialogueManager instance { get; private set; }
    
    [SerializeField] DiscussionSegment discussion;
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
        CharacterCourt prevCharacter = new CharacterCourt();
        if (conversationIndex > 0)
        {
            prevCharacter = discussion.discussionNodes[conversationIndex - 1].character;
        }
        DiscussionNode nextNode = discussion.discussionNodes[conversationIndex];
        CharacterStand characterStand = characterStands.Find(stand => stand.character == nextNode.character);
        if (!nextNode.usePrevCamera)
        {
            cameraController.TeleportToTarget(characterStand.transform, characterStand.heightPivot, nextNode.positionOffset, nextNode.rotationOffset, nextNode.fovOffset);
            effectController.Reset();
        }
        string line = ((VNTextData)nextNode.textData).text;
        DialogueSystem.instance.ShowSpeakerName(nextNode.character.displayName);

        if (conversationIndex == 0 || prevCharacter != nextNode.character)
        {
            ((CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator)).ChangeFace(nextNode.character.name);
        }
        
        List<string> lines = new List<string>();
        lines.Add(line);
        DialogueSystem.instance.Say(lines);
        
        
        
        foreach (CameraEffect cameraEffect in nextNode.cameraEffects)
        {
            effectController.StartEffect(cameraEffect);
        }

        List<Command> commands = ((nextNode.textData) as VNTextData).commands;
        if (commands != null)
        {
            foreach (Command command in commands)
            {
                command.Execute();
            } 
        }
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
