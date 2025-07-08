using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Dialogue Node Draw")]
public class ConversationNodeDraw : DrawNode
{
    public override void DrawWindow(DialogueNode b)
    {
        ConversationNode node = (ConversationNode)b;

        node.nodeRect.height = 80;
        node.nodeRect.width = 200;

        node.character = (CharacterCourt)EditorGUILayout.ObjectField(b.character, typeof(CharacterCourt), false);
        ShowCameraEffect(ref node.cameraEffects, ref node);
      
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
            node.textLines[i] = GUILayout.TextField(node.textLines[i]);
            node.nodeRect.height += 30;
            EditorGUILayout.Separator();
            b.nodeRect.height += 25;
        }
        
        
    }
    private void ShowCameraEffect(ref List<CameraEffect> cameraEffects, ref ConversationNode b)
    {
        for(int i = 0; i < cameraEffects.Count; i++)
        {
            GUILayout.BeginHorizontal();
            cameraEffects[i] = (CameraEffect)EditorGUILayout.ObjectField(cameraEffects[i], typeof(CameraEffect), false);
            if(GUILayout.Button("X", GUILayout.Width(20)))
            {
                cameraEffects.RemoveAt(i);
                continue;
            }
            b.nodeRect.height += 20;
            GUILayout.EndHorizontal();
        }
        if(GUILayout.Button("Add camera effect"))
        {
            cameraEffects.Add(null);
        }
        b.nodeRect.height += 20;
    }
}