using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DebateText
{
    public string text;
    public List<TextEffect> textEffects;
    public TextEffect introEffect;
    public TextEffect outroEffect;
    public Vector3 spawnOffset;
    public Vector3 rotationOffset;
    public Vector3 scale = Vector3.one;
    public float ttl = 1f;
    public Vector2 textLineScrollPosition;
    public Evidence correctEvidence;

    public DebateText()
    {
        text = "";
        textEffects = new List<TextEffect>();
        spawnOffset = Vector3.zero;
        ttl = 1f;
        textLineScrollPosition = new Vector2(0, 0);
    }
}

public class DebateTextData : TextData
{
    public List<DebateText> textLines;

    public DebateTextData()
    {
        textLines = new List<DebateText>();
    }
}