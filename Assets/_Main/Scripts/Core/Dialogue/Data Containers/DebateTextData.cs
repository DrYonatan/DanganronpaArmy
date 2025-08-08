using System;
using System.Collections.Generic;
using UnityEngine;

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

public class DebateTextData : TextData
{
    public List<TextLine> textLines;

    public DebateTextData()
    {
        textLines = new List<TextLine>();
    }
}