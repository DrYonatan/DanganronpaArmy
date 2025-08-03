using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Dialogue Node Draw")]
public class ConversationNodeDraw : TrialNodeDraw
{
    public override void DrawWindow(DialogueNode b)
    {
        base.DrawWindow(b);
        
        ConversationNode node = (ConversationNode)b;

        node.nodeRect.height = 300;
        node.nodeRect.width = 200;
        
      
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