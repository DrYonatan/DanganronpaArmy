
using System;
using UnityEngine;

namespace _Main.Scripts.Court
{
    [Serializable]
    public class DebateDialogueNode : DialogueNode
    {
        public Evidence evidence;
        public string statement;
        public Color statementColor;
        
        public DebateDialogueNode(DrawNode _drawNode) : base(_drawNode)
        {
        }
    }
}