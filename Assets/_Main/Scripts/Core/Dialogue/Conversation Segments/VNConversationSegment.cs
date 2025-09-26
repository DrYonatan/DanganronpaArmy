using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CameraLookDirection
{
    Left,
    MidLeft,
    Middle,
    MidRight,
    Right
}

[CreateAssetMenu(menuName = "Data/VN Conversation Segment")]
public class VNConversationSegment : ScriptableObject
{
    public ConversationSettings settings = new ();
    [SerializeReference] public List<DialogueNode> nodes = new List<DialogueNode>();
}
