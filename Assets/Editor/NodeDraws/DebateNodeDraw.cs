using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Debate Dialogue Node Draw")]
public class DebateNodeDraw : ConversationNodeDraw
{
    private Vector2 textLinesScrollPosition;

    public override void DrawWindow(DialogueNode b)
    {
        GUILayout.BeginHorizontal();
        base.DrawWindow(b);
        DebateNode node = (DebateNode)b;

        GUILayout.BeginVertical();
        node.evidence = (Evidence)EditorGUILayout.ObjectField(node.evidence, typeof(Evidence), false);
        node.statement = GUILayout.TextField(node.statement);
        node.voiceLine = (AudioClip)EditorGUILayout.ObjectField(node.voiceLine, typeof(AudioClip), false);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    protected override void ShowTextData(DialogueNode b)
    {
        DebateNode node = (DebateNode)b;
        DebateTextData textData = b.textData as DebateTextData;

        if (textData.textLines.Count == 0)
        {
            textData.textLines.Add(new TextLine());
        }
        
        for (int i = 0; i < textData.textLines.Count; i++)
        {
            TextLine textLine = textData.textLines[i];
            GUILayout.BeginVertical(GUILayout.Width(150));
            GUILayout.Label($"Line {i}#");
            textLine.text = GUILayout.TextField(textLine.text, GUILayout.Width(150));
            textLine.spawnOffset = EditorGUILayout.Vector3Field("Spawn Offset", textLine.spawnOffset);
            textLine.scale = EditorGUILayout.Vector3Field("Scale", textLine.scale);
            textLine.ttl = EditorGUILayout.FloatField("Time", textLine.ttl);
            ShowTextEffect(ref textLine.textEffect, ref node);
            
            GUILayout.EndVertical();
            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
            {
                textData.textLines.RemoveAt(i);
            }
            if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
            {
                textData.textLines.Insert(i + 1, new TextLine());
            }
        }
    }

    private void ShowTextEffect(ref List<TextEffect> textEffect, ref DebateNode b)
    {
        GUILayout.BeginScrollView(textLinesScrollPosition);
        for(int i = 0; i < textEffect.Count; i++)
        {
            GUILayout.BeginHorizontal();
            textEffect[i] = (TextEffect)EditorGUILayout.ObjectField(textEffect[i], typeof(TextEffect), false);
            if(GUILayout.Button("X", GUILayout.Width(20)))
            {
                textEffect.RemoveAt(i);
            }
            
            GUILayout.EndHorizontal();
        }
        if(GUILayout.Button("Add text effect"))
        {
            textEffect.Add(null);
        }
        
        GUILayout.EndScrollView();
    }
   
}