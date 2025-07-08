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

      //  node.cameraEffect = (CameraEffect)EditorGUILayout.ObjectField(b.cameraEffect, typeof(CameraEffect), false);


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
}