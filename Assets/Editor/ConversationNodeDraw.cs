using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Dialogue Node Draw")]
public class ConversationNodeDraw : TrialNodeDraw
{
    public override void DrawWindow(TrialDialogueNode b)
    {
        b.nodeRect.height = 300;
        b.nodeRect.width = 200;
        
        base.DrawWindow(b);
        
        ConversationNode node = (ConversationNode)b;
        ShowTextLines(node);
        ShowCommands(node);
    }

    private void ShowCommands(ConversationNode node)
    {
        List<Command> commands = node.commands;
        GUILayout.Label("Commands", EditorStyles.boldLabel);
        
        for (int i = 0; i < commands?.Count; i++)
        {
            var command = commands[i];

            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();

            GUILayout.Label(command.GetType().Name, EditorStyles.boldLabel);

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                commands.RemoveAt(i);
            }
            else
            {
                node.nodeRect.height += command.height;
            }

            GUILayout.EndHorizontal();

            command.DrawGUI(); // Call the command's GUI drawing logic

            GUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Command"))
        {
            
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Play Sound Effect"), false, () => AddCommand(node, new PlaySoundEffectCommand()));
            menu.AddItem(new GUIContent("Play Music"), false, () => AddCommand(node, new PlayMusicCommand()));
            // Add more as needed
            menu.ShowAsContext();
            
        }
    }

    private void AddCommand(ConversationNode node, Command command)
    {
        if(node.commands == null)
            node.commands = new List<Command>();
        node.commands.Add(command);
        
    }
    private void ShowTextLines(ConversationNode node)
    {
        
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
            node.nodeRect.height += 25;
        }
    }
}