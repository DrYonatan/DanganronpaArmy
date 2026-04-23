using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Conversation Segment")]
public class DiscussionSegment : TrialSegment
{
    [SerializeReference] public List<DiscussionNode> discussionNodes;
    public ConversationSettings settings;

    public override void Play()
    {
        TrialDialogueManager.instance.PlayDiscussion(this);
    }

    public override void HandleGameOver()
    {
        TrialDialogueManager.instance.isGameOvering = true;
    }
}