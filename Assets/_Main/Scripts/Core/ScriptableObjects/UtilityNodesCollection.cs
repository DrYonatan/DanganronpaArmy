using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterDefaultWrongNodes
{
    public Character character;
    public List<DiscussionNode> nodes;
}

[CreateAssetMenu(menuName = "Data/Utility Nodes Collection")]
public class UtilityNodesCollection : ScriptableObject
{
    public VNChoiceNode getOutOfRoom = new VNChoiceNode(null);
    
    public DiscussionNode wrongAnswer = new DiscussionNode(null);

    public DiscussionNode debateWrongEvidence = new DiscussionNode(null);
    
    public List<CharacterDefaultWrongNodes> characterDefaultWrongNodes = new List<CharacterDefaultWrongNodes>();
}
