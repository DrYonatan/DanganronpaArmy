using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DialogueNode 
{
    public Rect nodeRect;
    public string title;

    public DrawNode drawNode;


    public string text;
    public CharacterCourt character;

    public CameraEffect cameraEffect;
    
    public DialogueNode(DrawNode _drawNode)
    {
        drawNode = _drawNode;
    }

    public void DrawNode()
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this);
        }
    }
}
