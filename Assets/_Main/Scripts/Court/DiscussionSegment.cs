using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Conversation Segment")]
public class DiscussionSegment : TrialSegment
{
    public List<DiscussionNode> discussionNodes;

    public override void Play()
    {
        TrialDialogueManager.instance.PlayDiscussion(this);
    }
}