using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Debate Dialogue Node Draw")]
public class DebateDialogueNodeDraw : DialogueNodeDraw
{
    public override void DrawWindow(DialogueNode b)
    {
        DebateDialogueNode node = (DebateDialogueNode)b;
        node.evidence = (Evidence)EditorGUILayout.ObjectField(node.evidence, typeof(Evidence), false);
        node.statement = EditorGUILayout.TextField(node.statement);
        node.statementColor = EditorGUILayout.ColorField(node.statementColor);
        base.DrawWindow(b);
    }
}