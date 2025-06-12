using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Dialogue Node Draw")]
public class DialogueNodeDraw : DrawNode
{
    public override void DrawWindow(DialogueNode b)
    {
        b.nodeRect.height = 200;
        b.nodeRect.width = 200;
        
        b.character = (CharacterCourt)EditorGUILayout.ObjectField(b.character, typeof(CharacterCourt), false);

        b.cameraEffect = (CameraEffect)EditorGUILayout.ObjectField(b.cameraEffect, typeof(CameraEffect), false);

        b.voiceLine = (AudioClip)EditorGUILayout.ObjectField(b.voiceLine, typeof(AudioClip), false);

        b.expression = (CharacterState)EditorGUILayout.EnumPopup(b.expression);

        if(b.textLines == null) { return; }

        for(int i = 0; i < b.textLines.Count; i++)
        {
            EditorGUILayout.Separator();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Line #" + i.ToString());
            if (GUILayout.Button(("X"), GUILayout.Width(20)))
            {
                b.textLines.RemoveAt(i);
                continue;
            }
            GUILayout.EndHorizontal();
            b.textLines[i].text = GUILayout.TextField(b.textLines[i].text);
            b.textLines[i].spawnOffset = EditorGUILayout.Vector3Field("Offset", b.textLines[i].spawnOffset);
            b.textLines[i].scale = EditorGUILayout.Vector3Field("Scale", b.textLines[i].scale);
            b.textLines[i].ttl = EditorGUILayout.FloatField("Time", b.textLines[i].ttl);
            b.nodeRect.height += 134;
            EditorGUILayout.Separator();
            b.nodeRect.height += 16;
            ShowTextEffect(ref b.textLines[i].textEffect, ref b);
        }

    }

    private void ShowTextEffect(ref List<TextEffect> textEffect, ref DialogueNode b)
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
