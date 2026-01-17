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

    private Coroutine runningNodesRoutine;

    public bool isFinishedRunningNodes;

    private void Awake()
    {
        instance = this;
    }
    
    public void PlayDiscussion(DiscussionSegment discussion)
    {
        SetTextBox();
        DialogueSystem.instance.dialogueBoxAnimator.gameObject.SetActive(true);
        ImageScript.instance.UnFadeToBlack(0.5f);
        animator.ChangeFace(null);
        animator.FaceAppear();
        animator.AnimateBackgroundText();
        
        StartCoroutine(RunDiscussion(discussion));
    }

    public void SetTextBox()
    {
        DialogueSystem.instance.SetTextBox(dialogueContainer);
    }

    IEnumerator RunDiscussion(DiscussionSegment discussion)
    {
        yield return RunNodes(discussion.discussionNodes);
        HandleConversationEnd(discussion);
    }

    public IEnumerator RunNodes(List<DiscussionNode> nodes)
    {
        runningNodesRoutine = StartCoroutine(StartNodes(nodes));
        yield return new WaitUntil(() => isFinishedRunningNodes);
    }

    private IEnumerator StartNodes(List<DiscussionNode> nodes)
    {
        isFinishedRunningNodes = false;
        for (int i = currentLineIndex; i < nodes.Count; i++)
        {
            yield return nodes[i].Play();
            currentLineIndex++;
        }

        isFinishedRunningNodes = true;
        currentLineIndex = 0;
    }

    private void HandleConversationEnd(DiscussionSegment discussion)
    {
        ConversationEnd();
        
        discussion.Finish();
    }

    public void ConversationEnd()
    {
        DialogueSystem.instance.dialogueBoxAnimator.TextBoxDisappear();
        effectController.Reset();
        currentLineIndex = 0;
    }

    public void StopConversation()
    {
        StopCoroutine(runningNodesRoutine);
    }
    
}
