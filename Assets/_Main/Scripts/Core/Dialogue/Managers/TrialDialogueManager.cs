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

    public bool isGameOvering;

    private void Awake()
    {
        instance = this;
    }
    
    public void PlayDiscussion(DiscussionSegment discussion)
    {
        SetTextBox();
        isGameOvering = false;
        DialogueSystem.instance.dialogueBoxAnimator.gameObject.SetActive(true);
        ImageScript.instance.UnFadeToBlack(0.5f);
        animator.TextBoxAppear();
        animator.ChangeFace(null);
        animator.FaceAppear();
        animator.AnimateBackgroundText();
        
        runningNodesRoutine = StartCoroutine(RunDiscussion(discussion));
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
        StartCoroutine(StartNodes(nodes));
        yield return new WaitUntil(() => isFinishedRunningNodes);
    }

    private IEnumerator StartNodes(List<DiscussionNode> nodes)
    {
        isFinishedRunningNodes = false;
        for (int i = currentLineIndex; i < nodes.Count; i++)
        {
            if (isGameOvering)
            {
                StopConversation();
                yield return TrialManager.instance.GameOver();
                yield break;
            }
            
            yield return nodes[i].Play();
            currentLineIndex++;
        }

        isFinishedRunningNodes = true;
        currentLineIndex = 0;
    }

    public IEnumerator PlayNodeList(List<DiscussionNode> nodes)
    {
        foreach (DiscussionNode node in nodes)
        {
            yield return node.Play();
        }
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
