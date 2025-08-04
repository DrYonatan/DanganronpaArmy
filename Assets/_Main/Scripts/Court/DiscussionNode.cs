using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiscussionNode : TrialDialogueNode
{
    [SerializeReference]
    public List<Command> commands = new();

    public DiscussionNode(DrawNode _drawNode) : base(_drawNode)
    {
        textData = new VNTextData();
        commands = new List<Command>();
    }

    public void DrawNode()
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this);
        }
    }
}