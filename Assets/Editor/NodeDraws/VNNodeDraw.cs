using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Dialogue Node Draw")]
public class VNNodeDraw : DrawNode
{
    public override void DrawWindow(DialogueNode b, ConversationSettings settings, float windowWidth, float windowHeight)
    {
        GUILayout.BeginHorizontal();
        
        GUILayout.BeginVertical(GUILayout.Width(300));

        b.character = (Character)EditorGUILayout.ObjectField(b.character, typeof(Character), false);
        if (b.character != null && b.character.emotions != null && b.character.emotions.Count > 0)
        {
            // Get list of emotion names
            string[] options = b.character.emotions.Select(e => e.name).ToArray();

            // Find current index of the selected state
            int currentIndex = b.expressionIndex;

            // Draw popup
            int newIndex = EditorGUILayout.Popup("Expression", Mathf.Max(0, currentIndex), options);

            // Assign selected state
            b.expressionIndex = newIndex;
        }
        else
        {
            EditorGUILayout.LabelField("No emotions defined for this character.");
        }        ShowPreviewImage(b);
        GUILayout.EndVertical();
        
        ShowTextData(b, windowWidth * 0.5f);
        
        GUILayout.EndHorizontal();
    }

    protected virtual void ShowTextData(DialogueNode b, float width)
    {
        VNTextData vnTextData = (VNTextData)b.textData;
        vnTextData.text = GUILayout.TextArea(vnTextData.text, GUILayout.Height(180),  GUILayout.Width(width));
        GUILayout.BeginVertical();
        ShowCommands(b);
        GUILayout.EndVertical();
    }
    
    private void ShowCommands(DialogueNode node)
    {
        List<Command> commands = ((VNTextData)node.textData).commands;
        GUILayout.Label("Commands", EditorStyles.boldLabel);
        
        node.commandsScrollPosition = GUILayout.BeginScrollView(node.commandsScrollPosition, GUILayout.Height(150));
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

            GUILayout.EndHorizontal();

            command.DrawGUI(); // Call the command's GUI drawing logic

            GUILayout.EndVertical();
        }
        
        GUILayout.EndScrollView();

        if (GUILayout.Button("Add Command"))
        {
            
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Play Sound Effect"), false, () => AddCommand(node, new PlaySoundEffectCommand()));
            menu.AddItem(new GUIContent("Play Music"), false, () => AddCommand(node, new PlayMusicCommand()));
            menu.AddItem(new GUIContent("Play Ultimate Animation"), false, () => AddCommand(node, new PlayUltimateAnimation()));
            menu.AddItem(new GUIContent("Select Evidence"), false, () => AddCommand(node, new PromptEvidenceSelection()));
            menu.AddItem(new GUIContent("Show Popup"), false, () => AddCommand(node, new ShowPopup()));
            menu.AddItem(new GUIContent("Remove Popup"), false, () => AddCommand(node, new RemovePopup()));
            // Add more as needed
            menu.ShowAsContext();
        }
        
    }
    
    private void AddCommand(DialogueNode node, Command command)
    {
        VNTextData vnTextData = (VNTextData)node.textData;

        if (node.textData == null)
        {
            vnTextData.commands = new List<Command>();
        }
        vnTextData.commands.Add(command);
        
    }

    protected virtual void ShowPreviewImage(DialogueNode node)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (node.character != null && node.character.faceSprite != null)
            GUILayout.Label(node.character.faceSprite.texture, GUILayout.Width(150), GUILayout.Height(150));
        else
            GUILayout.Box("", GUILayout.Width(150), GUILayout.Height(150));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
