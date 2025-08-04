using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Conversation Segment")]
public class DiscussionSegment : ScriptableObject
{
    public List<DiscussionNode> discussionNodes;
}