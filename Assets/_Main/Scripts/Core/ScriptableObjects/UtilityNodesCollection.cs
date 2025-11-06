using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterDefaultWrongNodes
{
    public Character character;
    [SerializeReference] public List<DiscussionNode> nodes = new List<DiscussionNode>();
}

[CreateAssetMenu(menuName = "Data/Utility Nodes Collection")]
public class UtilityNodesCollection : ScriptableObject
{
    public List<DialogueNode> getOutOfRoom = new List<DialogueNode>();
    
    [SerializeReference] public List<DiscussionNode> wrongAnswer = new List<DiscussionNode>();

    [SerializeReference] public List<DiscussionNode> debateWrongEvidence = new List<DiscussionNode>();
    
    public List<CharacterDefaultWrongNodes> characterDefaultWrongNodes = new List<CharacterDefaultWrongNodes>();

    [SerializeReference] public List<DiscussionNode> gameOverNodes = new List<DiscussionNode>();
    
    public List<DialogueNode> wrongComicNodes = new List<DialogueNode>();
}
