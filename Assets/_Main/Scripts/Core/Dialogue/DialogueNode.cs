using System;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public Rect nodeRect;
    public string title;
    public DrawNode drawNode;
    
    public Character VnCharacter;
    public CharacterState expression;
    
    [SerializeReference]
    public TextData textData;
    
    protected DialogueNode(DrawNode drawNode)
    {
        this.drawNode = drawNode;
    }

    public void DrawNode()
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this);
        }
    }
}
