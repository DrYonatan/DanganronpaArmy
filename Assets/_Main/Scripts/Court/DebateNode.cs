using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.Court
{
    [Serializable]
    public class TextLine
    {
        public string text;
        public List<TextEffect> textEffect;
        public Vector3 spawnOffset;
        public Vector3 scale = Vector3.one;
        public float ttl = 1f;

        public TextLine()
        {
            text = "";
            textEffect = new List<TextEffect>();
            spawnOffset = Vector3.zero;
            ttl = 1f;
        }
    }
    
    [Serializable]
    public class DebateNode : TrialDialogueNode
    {
        public Evidence evidence;
        public string statement;
        public Color statementColor;
        public List<TextLine> textLines;
        public AudioClip voiceLine;

        public DebateNode(DrawNode _drawNode) : base(_drawNode)
        {
            textLines = new List<TextLine>();
        }
    }
}