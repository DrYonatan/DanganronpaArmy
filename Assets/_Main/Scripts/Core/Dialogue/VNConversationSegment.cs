using System;
using System.Collections;
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

[Serializable]
public class VNCharacterInfo
{
    public CharacterCourt Character;
    public CameraLookDirection LookDirection;
}

[CreateAssetMenu(menuName = "Data/VN Conversation Segment")]
public class VNConversationSegment : ScriptableObject
{
    public List<VNCharacterInfo> CharacterInfos = new ();
    public List<DialogueNode> nodes = new List<DialogueNode>();
}
