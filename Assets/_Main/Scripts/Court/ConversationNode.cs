using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConversationNode : TrialDialogueNode
{
    public List<string> textLines;
    [SerializeReference]
    public List<Command> commands = new();

    public ConversationNode(DrawNode _drawNode) : base(_drawNode)
    {
        textLines = new List<string>();
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