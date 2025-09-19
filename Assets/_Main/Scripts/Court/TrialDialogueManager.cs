using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEngine.Android;

public class TrialDialogueManager : MonoBehaviour
{
    public static TrialDialogueManager instance { get; private set; }
    
    [SerializeField] CameraEffectController effectController;
    [SerializeField] CameraController cameraController;
    
    public DialogueContainer dialogueContainer;

    private void Awake()
    {
        instance = this;
    }
    
    public void PlayDiscussion(DiscussionSegment discussion)
    {
        DialogueSystem.instance.SetTextBox(dialogueContainer);
        DialogueSystem.instance.dialogueBoxAnimator.gameObject.SetActive(true);
        ImageScript.instance.UnFadeToBlack(0.5f);
        CourtTextBoxAnimator animator = ((CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator));
        animator.ChangeFace(null);
        animator.FaceAppear();
        animator.TextBoxAppear();
        
        StartCoroutine(RunDiscussion(discussion));
    }

    IEnumerator RunDiscussion(DiscussionSegment discussion)
    {
        yield return RunNodes(discussion.discussionNodes);
        HandleConversationEnd(discussion);
    }

    public IEnumerator RunNodes(List<DiscussionNode> nodes)
    {
        foreach (DiscussionNode discussionNode in nodes)
        {
            yield return PlayConversationNode(discussionNode);
        }
    }

    IEnumerator PlayConversationNode(DiscussionNode node)
    {
        CharacterStand characterStand = TrialManager.instance.characterStands.Find(stand => stand.character == node.character);
        if (!node.usePrevCamera)
        {
            cameraController.TeleportToTarget(characterStand.transform, characterStand.heightPivot, node.positionOffset, node.rotationOffset, node.fovOffset);
            effectController.Reset();
        }

        ((CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator)).ChangeFace(node.character.faceSprite);
        
        foreach (CameraEffect cameraEffect in node.cameraEffects)
        {
            effectController.StartEffect(cameraEffect);
        }
        characterStand.SetSprite(node.expression);

        yield return DialogueSystem.instance.Say(node);

    }

    public void HandleConversationEnd(DiscussionSegment discussion)
    {
        DialogueSystem.instance.dialogueBoxAnimator.TextBoxDisappear();
        effectController.Reset();
        
        discussion.Finish();
    }
    
}
