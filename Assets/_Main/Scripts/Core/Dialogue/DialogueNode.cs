using System;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public Rect nodeRect;
    public string title;
    DrawNode drawNode;
    
    public Character character;
    public CharacterState characterState;
    
    public TextData textData;
    
    protected DialogueNode(DrawNode drawNode)
    {
        this.drawNode = drawNode;
    }

    public void DrawNode()
    {
        if (drawNode != null)
        {
            
        }
    }
}
