using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConversationNode : DialogueNode
{
    public List<string> textLines;

    public ConversationNode(DrawNode _drawNode) : base(_drawNode)
    {
        textLines = new List<string>();
    }

    public void DrawNode()
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this);
        }
    }
}