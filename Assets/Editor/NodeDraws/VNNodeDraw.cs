using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Dialogue Node Draw")]
public class VNNodeDraw : DrawNode
{
    private Vector2 scrollPosition;

    public override void DrawWindow(DialogueNode b)
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = Texture2D.grayTexture;
        
        GUILayout.BeginHorizontal(style, GUILayout.Width(1400));
        
        GUILayout.BeginVertical(GUILayout.Width(300));
        b.character = (CharacterCourt)EditorGUILayout.ObjectField(b.character, typeof(CharacterCourt), false);
        b.expression = (CharacterState)EditorGUILayout.EnumPopup(b.expression);
        ShowPreviewImage(b);
        GUILayout.EndVertical();
        
        VNTextData vnTextData = (VNTextData)b.textData;
        vnTextData.text = GUILayout.TextArea(vnTextData.text, GUILayout.Height(180),  GUILayout.Width(800));
        GUILayout.BeginVertical();
        ShowCommands(b);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        
    }
    
    private void ShowCommands(DialogueNode node)
    {
        List<Command> commands = ((VNTextData)node.textData).commands;
        GUILayout.Label("Commands", EditorStyles.boldLabel);
        
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(150));
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
        GUILayout.Box("", GUILayout.Width(150), GUILayout.Height(150));
    }
}
