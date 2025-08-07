using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiscussionNode : TrialDialogueNode
{

    public DiscussionNode(DrawNode _drawNode) : base(_drawNode)
    {
        textData = new VNTextData();
    }

    public void DrawNode()
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this);
        }
    }
}