using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/VN Conversation Segment")]
public class VNConversationSegment : ScriptableObject
{
    public List<DialogueNode> nodes = new List<DialogueNode>();
}
