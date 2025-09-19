using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TrialDialogueManager : MonoBehaviour
{
    public static TrialDialogueManager instance { get; private set; }
    
    [SerializeField] public CameraEffectController effectController;
    [SerializeField] public CameraController cameraController;
    [SerializeField] public List<CharacterStand> characterStands;
    
    public DialogueContainer dialogueContainer;

    private void Awake()
    {
        instance = this;
    }
    
    public void PlayDiscussion(DiscussionSegment discussion)
    {
        DialogueSystem.instance.SetTextBox(dialogueContainer);
        DialogueSystem.instance.dialogueBoxAnimator.gameObject.SetActive(true);
        CourtTextBoxAnimator animator = ((CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator));
        animator.ChangeFace(null);
        animator.FaceAppear();
        
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
            yield return discussionNode.Play();
        }
    }

    public void HandleConversationEnd(DiscussionSegment discussion)
    {
        DialogueSystem.instance.dialogueBoxAnimator.TextBoxDisappear();
        effectController.Reset();
        
        discussion.Finish();
    }
    
}
