using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.Court
{
    [Serializable]
    public class DebateNode : DiscussionNode
    {
        public string statement;
        public AudioClip voiceLine;
        public Vector2 textLinesScrollPosition;

        public DebateNode(DrawNode _drawNode) : base(_drawNode)
        {
        }


        protected override void InitializeTextData()
        {
            textData = new DebateTextData();
        }
    }
}