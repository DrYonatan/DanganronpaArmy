using System;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public Rect nodeRect;
    public Vector2 commandsScrollPosition;

    public string title;
    public DrawNode drawNode;
    
    public CharacterCourt character;

    public CharacterState expression;
    
    [SerializeReference]
    public TextData textData;
    
    public DialogueNode(DrawNode drawNode)
    {
        this.drawNode = drawNode;
        this.InitializeTextData();
    }

    public void DrawNode(float windowWidth, float windowHeight)
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this, windowWidth, windowHeight);
        }
    }

    protected virtual void InitializeTextData()
    {
        this.textData = new VNTextData();
    }
}
