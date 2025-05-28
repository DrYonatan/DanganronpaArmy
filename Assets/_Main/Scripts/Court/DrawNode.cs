using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class DrawNode : ScriptableObject
{
    public abstract void DrawWindow(DialogueNode b);
}
