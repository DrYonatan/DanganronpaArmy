using System;
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
        style.padding = new RectOffset(2, 2, 2, 2);
        
        GUILayout.BeginVertical(style);
        
        GUILayout.BeginHorizontal(style);

        if (node.textLinesPage * 2 >= ((DebateTextData)node.textData).textLines.Count)
        {
            node.textLinesPage = Math.Max(node.textLinesPage-1, 0);
        }
        
        if (GUILayout.Button("<") && node.textLinesPage > 0)
        {
            node.textLinesPage--;
        }
        if (GUILayout.Button(">") && node.textLinesPage < ((DebateTextData)node.textData).textLines.Count-1)
        {
            node.textLinesPage++;
        }
        GUILayout.EndHorizontal();
        
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

        if (textData == null)
            return;

        if (textData.textLines.Count == 0)
        {
            textData.textLines.Add(new DebateText());
        }
        
        GUILayout.BeginHorizontal(GUILayout.Width(1000));
        
        for (int i = node.textLinesPage * 2; i < Mathf.Min(node.textLinesPage * 2 + 2, textData.textLines.Count); i++)
        {
            ShowSingleTextData(node, textData, i, style);
        }
        GUILayout.EndHorizontal();
        
    }

    private void ShowSingleTextData(DebateNode node, DebateTextData textData, int i, GUIStyle style)
    {
        DebateText debateText = textData.textLines[i];
        GUILayout.BeginVertical();
            
        GUILayout.BeginHorizontal(style, GUILayout.Width(400));
        GUILayout.Label($"Line {i}#");
        if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
        {
            textData.textLines.RemoveAt(i);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(style, GUILayout.Width(400));
            
        GUILayout.BeginVertical(GUILayout.Width(180));
        debateText.text = GUILayout.TextArea(debateText.text, GUILayout.Width(150));
        debateText.spawnOffset = DrawCustomVector3Input(debateText.spawnOffset, 140, "Spawn Offset");
        GUILayout.Space(5);
        debateText.rotationOffset = DrawCustomVector3Input(debateText.rotationOffset, 140, "Rotation Offset");
        // debateText.scale = DrawCustomVector3Input(debateText.scale, 140, "Scale");
        debateText.ttl = EditorGUILayout.FloatField("Time", debateText.ttl);
        GUILayout.EndVertical();
            
        GUILayout.BeginVertical();
        debateText.correctEvidence = (Evidence)EditorGUILayout.ObjectField("Evidence", debateText.correctEvidence, typeof(Evidence), false);
        ShowTextEffect(ref debateText, ref node);
        GUILayout.EndVertical();
        if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(100)))
        {
            textData.textLines.Insert(i + 1, new DebateText());
        }
        GUILayout.EndHorizontal();
            
        GUILayout.EndVertical();
    }

    private void ShowTextEffect(ref DebateText debateText, ref DebateNode b)
    {
        if(GUILayout.Button("Add text effect"))
        {
            debateText.textEffects.Add(null);
        }
        
        debateText.textLineScrollPosition = GUILayout.BeginScrollView(debateText.textLineScrollPosition, GUILayout.Height(120));
        
        for(int i = 0; i < debateText.textEffects.Count; i++)
        {
            GUILayout.BeginHorizontal();
            debateText.textEffects[i] = (TextEffect)EditorGUILayout.ObjectField(debateText.textEffects[i], typeof(TextEffect), false);
            if(GUILayout.Button("X", GUILayout.Width(20)))
            {
                debateText.textEffects.RemoveAt(i);
            }
            
            GUILayout.EndHorizontal();
        }
       
        
        GUILayout.EndScrollView();
    }

    private Vector3 DrawCustomVector3Input(Vector3 inputVector, float width, string  label)
    {
        Color oldBg = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        
        EditorGUIUtility.labelWidth = 12;
        GUIStyle style = new GUIStyle();
        style.normal.background = Texture2D.grayTexture;
        
        GUILayout.BeginVertical(style);
        GUILayout.Label(label);
        GUILayout.BeginHorizontal(GUILayout.Width(width));
        inputVector.x =  EditorGUILayout.FloatField("X", inputVector.x, GUILayout.Width(width/3));
        inputVector.y =  EditorGUILayout.FloatField("Y", inputVector.y, GUILayout.Width(width/3));
        inputVector.z =  EditorGUILayout.FloatField("Z", inputVector.z, GUILayout.Width(width/3));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        
        EditorGUIUtility.labelWidth = 0; // return to default
        
        GUI.backgroundColor = oldBg;

        return inputVector;
    }
}
