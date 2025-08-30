using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Dialogue Node Draw")]
public class VNNodeDraw : DrawNode
{
    public override void DrawWindow(DialogueNode b, float windowWidth, float windowHeight)
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = Texture2D.grayTexture;
        
        GUILayout.BeginHorizontal(style);
        
        GUILayout.BeginVertical(GUILayout.Width(300));
        b.character = (CharacterCourt)EditorGUILayout.ObjectField(b.character, typeof(CharacterCourt), false);
        b.expression = (CharacterState)EditorGUILayout.EnumPopup(b.expression);
        ShowPreviewImage(b);
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
        if (node.character.faceSprite != null)
            GUILayout.Label(node.character.faceSprite.texture, GUILayout.Width(150), GUILayout.Height(150));
        else
            GUILayout.Box("", GUILayout.Width(150), GUILayout.Height(150));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
