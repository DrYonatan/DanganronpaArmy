using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Conversation Segment")]
public class ConversationSegment : ScriptableObject
{
    public List<ConversationNode> conversationNodes;
}