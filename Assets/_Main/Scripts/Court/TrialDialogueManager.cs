using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TrialDialogueManager : MonoBehaviour, IConversationNodePlayer
{
    public static TrialDialogueManager instance { get; private set; }
    
    [SerializeField] DiscussionSegment discussion;
    [SerializeField] CameraEffectController effectController;
    [SerializeField] CameraController cameraController;
    [SerializeField] List<CharacterStand> characterStands;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        List<DialogueNode> nodes = new List<DialogueNode>();
        foreach (DiscussionNode discussionNode in discussion.discussionNodes)
        {
            nodes.Add(discussionNode);
        }
       DialogueSystem.instance.Say(nodes);
    }

    

    public void PlayConversationNode(int conversationIndex)
    {
        CharacterCourt prevCharacter = ScriptableObject.CreateInstance<CharacterCourt>();
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

        if (conversationIndex == 0 || prevCharacter != nextNode.character)
        {
            ((CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator)).ChangeFace(nextNode.character.name);
        }
  
        
        foreach (CameraEffect cameraEffect in nextNode.cameraEffects)
        {
            effectController.StartEffect(cameraEffect);
        }
        
    }

    public void StartConversation(VNConversationSegment conversationSegment)
    {
        throw new System.NotImplementedException();
    }

    public void HandleConversationEnd()
    {
        // Should be something like GoToNextCourtEvent()
    }
    
}
