using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TrialDialogueManager : MonoBehaviour
{
    public static TrialDialogueManager instance { get; private set; }
    
    [SerializeField] public CameraEffectController effectController;
    [SerializeField] public CameraController cameraController;
    
    public DialogueContainer dialogueContainer;

    public GotItAnimator gotItAnimator;

    public CourtTextBoxAnimator animator;

    public int currentLineIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        animator = (CourtTextBoxAnimator)DialogueSystem.instance.dialogueBoxAnimator;
    }
    
    public void PlayDiscussion(DiscussionSegment discussion)
    {
        DialogueSystem.instance.SetTextBox(dialogueContainer);
        DialogueSystem.instance.dialogueBoxAnimator.gameObject.SetActive(true);
        ImageScript.instance.UnFadeToBlack(0.5f);
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
        for (int i = currentLineIndex; i < nodes.Count; i++)
        {
            yield return nodes[i].Play();
            currentLineIndex++;
        }
    }

    private void HandleConversationEnd(DiscussionSegment discussion)
    {
        DialogueSystem.instance.dialogueBoxAnimator.TextBoxDisappear();
        effectController.Reset();
        currentLineIndex = 0;
        
        discussion.Finish();
    }
    
}
