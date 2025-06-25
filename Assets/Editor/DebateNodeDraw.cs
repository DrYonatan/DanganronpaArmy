using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Debate Dialogue Node Draw")]
public class DebateNodeDraw : DrawNode
{
    public override void DrawWindow(DialogueNode b)
    {
        
        DebateNode node = (DebateNode)b;
        
        node.nodeRect.height = 200;
        node.nodeRect.width = 200;
        
        node.character = (CharacterCourt)EditorGUILayout.ObjectField(node.character, typeof(CharacterCourt), false);

        node.evidence = (Evidence)EditorGUILayout.ObjectField(node.evidence, typeof(Evidence), false);

        node.statement = EditorGUILayout.TextField(node.statement);

        node.statementColor = EditorGUILayout.ColorField(node.statementColor);

        node.cameraEffect = (CameraEffect)EditorGUILayout.ObjectField(node.cameraEffect, typeof(CameraEffect), false);

        node.voiceLine = (AudioClip)EditorGUILayout.ObjectField(node.voiceLine, typeof(AudioClip), false);

        node.expression = (CharacterState)EditorGUILayout.EnumPopup(node.expression);

        if(node.textLines == null) { return; }

        for (int i = 0; i < node.textLines.Count; i++)
        {
            EditorGUILayout.Separator();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Line #" + i.ToString());
            if (GUILayout.Button(("X"), GUILayout.Width(20)))
            {
                node.textLines.RemoveAt(i);
                GUILayout.EndHorizontal();
                continue;
            }

            GUILayout.EndHorizontal();
            node.textLines[i].text = GUILayout.TextField(node.textLines[i].text);
            node.textLines[i].spawnOffset = EditorGUILayout.Vector3Field("Offset", node.textLines[i].spawnOffset);
            node.textLines[i].scale = EditorGUILayout.Vector3Field("Scale", node.textLines[i].scale);
            node.textLines[i].ttl = EditorGUILayout.FloatField("Time", node.textLines[i].ttl);
            node.nodeRect.height += 134;
            EditorGUILayout.Separator();
            b.nodeRect.height += 16;
            ShowTextEffect(ref node.textLines[i].textEffect, ref node);
        }

    }
    
    private void ShowTextEffect(ref List<TextEffect> textEffect, ref DebateNode b)
    {
        for(int i = 0; i < textEffect.Count; i++)
        {
            GUILayout.BeginHorizontal();
            textEffect[i] = (TextEffect)EditorGUILayout.ObjectField(textEffect[i], typeof(TextEffect), false);
            if(GUILayout.Button("X", GUILayout.Width(20)))
            {
                textEffect.RemoveAt(i);
                continue;
            }
            b.nodeRect.height += 20;
            GUILayout.EndHorizontal();
        }
        if(GUILayout.Button("Add text effect"))
        {
            textEffect.Add(null);
        }
        b.nodeRect.height += 20;
    }
}