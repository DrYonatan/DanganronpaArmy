using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.Court
{
    [Serializable]
    public class DebateNode : DiscussionNode
    {
        public Evidence evidence;
        public string statement;
        public Color statementColor;
        public AudioClip voiceLine;

        public DebateNode(DrawNode _drawNode) : base(_drawNode)
        {
            textData = new DebateTextData();
        }
    }
}