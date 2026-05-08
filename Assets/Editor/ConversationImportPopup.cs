using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ConversationImportPopup : PopupWindowContent
{
    private readonly VNDialogueEditor editor;
    private Vector2 scroll;
    private string pastedText = "";

    public ConversationImportPopup(VNDialogueEditor editor)
    {
        this.editor = editor;
    }

    public override Vector2 GetWindowSize()
    {
        return new Vector2(600, 500);
    }

    public override void OnGUI(Rect rect)
    {
        EditorGUILayout.LabelField("Import Conversation", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        EditorGUILayout.HelpBox(
            "Format:\nCharacterName: Dialogue Text\n\nLeave an empty line between dialogue entries.",
            MessageType.Info
        );

        scroll = EditorGUILayout.BeginScrollView(scroll);

        pastedText = EditorGUILayout.TextArea(
            pastedText,
            GUILayout.ExpandHeight(true)
        );

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Generate Conversation From Text", GUILayout.Height(40)))
        {
            GenerateConversation();
        }
    }

    private void GenerateConversation()
    {
        if (string.IsNullOrWhiteSpace(pastedText))
            return;

        if (editor.nodes == null)
        {
            editor.nodes = new List<DialogueNode>();
        }

        editor.nodes.Clear();

        Character[] allCharacters = AssetDatabase
            .FindAssets("t:Character", new[] { "Assets/_Main/Data/Characters" })
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<Character>(path))
            .Where(character => character != null)
            .ToArray();

        string[] blocks = pastedText.Split(
            new[] { "\r\n\r\n", "\n\n" },
            System.StringSplitOptions.RemoveEmptyEntries
        );
        foreach (string rawBlock in blocks)
        {
            string block = rawBlock.Trim();

            int separatorIndex = block.IndexOf(':');

            if (separatorIndex == -1)
                continue;

            string characterAlt = block.Substring(0, separatorIndex).Trim();
            string dialogueText = block.Substring(separatorIndex + 1).Trim();

            Character matchingCharacter = allCharacters.FirstOrDefault(character =>
                character.alt == characterAlt
            );

            DialogueNode node = new DialogueNode(editor.textNode);

            node.character = matchingCharacter;

            VNTextData data = node.textData as VNTextData;

            if (data != null)
            {
                data.text = dialogueText;
            }

            editor.nodes.Add(node);
        }

        EditorUtility.SetDirty(editor.segment);
        AssetDatabase.SaveAssets();

        editor.Repaint();
        editorWindow?.Close();
    }
}