using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Debate Dialogue Node Draw")]
public class DebateNodeDraw : DiscussionNodeDraw
{
    public override void DrawWindow(DialogueNode b, ConversationSettings settings, float windowWidth, float windowHeight)
    {
        GUILayout.BeginHorizontal();
        base.DrawWindow(b, settings, windowWidth, windowHeight);
        DebateNode node = (DebateNode)b;

        GUIStyle style = new GUIStyle();
        style.normal.background = Texture2D.linearGrayTexture;
        
        GUILayout.BeginVertical(style);
        node.evidence = (Evidence)EditorGUILayout.ObjectField(node.evidence, typeof(Evidence), false);
        node.statement = GUILayout.TextArea(node.statement);
        node.voiceLine = (AudioClip)EditorGUILayout.ObjectField(node.voiceLine, typeof(AudioClip), false);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    protected override void ShowTextData(DialogueNode b, float width)
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = Texture2D.linearGrayTexture;
        
        DebateNode node = (DebateNode)b;
        DebateTextData textData = b.textData as DebateTextData;

        if (textData.textLines.Count == 0)
        {
            textData.textLines.Add(new DebateText());
        }
        
        node.textLinesScrollPosition = GUILayout.BeginScrollView(node.textLinesScrollPosition, GUILayout.Width(width));
        GUILayout.BeginHorizontal();
        for (int i = 0; i < textData.textLines.Count; i++)
        {
            DebateText debateText = textData.textLines[i];
            GUILayout.BeginVertical(style, GUILayout.Width(150));
            GUILayout.Label($"Line {i}#");
            debateText.text = GUILayout.TextArea(debateText.text, GUILayout.Width(150));
            debateText.spawnOffset = EditorGUILayout.Vector3Field("Spawn Offset", debateText.spawnOffset);
            debateText.scale = EditorGUILayout.Vector3Field("Scale", debateText.scale);
            debateText.ttl = EditorGUILayout.FloatField("Time", debateText.ttl);
            ShowTextEffect(ref debateText, ref node);
            
            GUILayout.EndVertical();
            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
            {
                textData.textLines.RemoveAt(i);
            }
            if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
            {
                textData.textLines.Insert(i + 1, new DebateText());
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
    }

    private void ShowTextEffect(ref DebateText debateText, ref DebateNode b)
    {
        debateText.textLineScrollPosition = GUILayout.BeginScrollView(debateText.textLineScrollPosition);
        for(int i = 0; i < debateText.textEffect.Count; i++)
        {
            GUILayout.BeginHorizontal();
            debateText.textEffect[i] = (TextEffect)EditorGUILayout.ObjectField(debateText.textEffect[i], typeof(TextEffect), false);
            if(GUILayout.Button("X", GUILayout.Width(20)))
            {
                debateText.textEffect.RemoveAt(i);
            }
            
            GUILayout.EndHorizontal();
        }
        if(GUILayout.Button("Add text effect"))
        {
            debateText.textEffect.Add(null);
        }
        
        GUILayout.EndScrollView();
    }
   
}