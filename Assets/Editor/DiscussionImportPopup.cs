using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DiscussionImportPopup : PopupWindowContent
{
    private const string PresetsFolder = "Assets/_Main/Data/Effects/Camera Presets";

    private readonly ConversationEditor editor;
    private Vector2 scroll;
    private string pastedText = "";

    public DiscussionImportPopup(ConversationEditor editor)
    {
        this.editor = editor;
    }

    public override Vector2 GetWindowSize()
    {
        return new Vector2(600, 500);
    }

    public override void OnGUI(Rect rect)
    {
        EditorGUILayout.LabelField("Import Discussion", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        EditorGUILayout.HelpBox(
            "Format:\nCharacterName: Dialogue Text\n\nLeave an empty line between dialogue entries.",
            MessageType.Info
        );

        EditorGUILayout.HelpBox(
            "Each node gets a random camera preset, which handles all offsets and effects. Consecutive lines from the same character will reuse the previous camera.",
            MessageType.Info
        );

        scroll = EditorGUILayout.BeginScrollView(scroll);

        pastedText = EditorGUILayout.TextArea(
            pastedText,
            GUILayout.ExpandHeight(true)
        );

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Generate Discussion From Text", GUILayout.Height(40)))
        {
            GenerateDiscussion();
        }
    }

    private void GenerateDiscussion()
    {
        if (string.IsNullOrWhiteSpace(pastedText))
            return;

        if (editor.discussionNodes == null)
        {
            editor.discussionNodes = new List<DiscussionNode>();
        }

        editor.discussionNodes.Clear();

        Character[] allCharacters = AssetDatabase
            .FindAssets("t:Character", new[] { "Assets/_Main/Data/Characters" })
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<Character>(path))
            .Where(character => character != null)
            .ToArray();

        CameraPreset[] allPresets = AssetDatabase
            .FindAssets("t:CameraPreset", new[] { PresetsFolder })
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<CameraPreset>(path))
            .Where(preset => preset != null)
            .ToArray();

        string[] blocks = pastedText.Split(
            new[] { "\r\n\r\n", "\n\n" },
            System.StringSplitOptions.RemoveEmptyEntries
        );
        DiscussionNode previousNode = null;
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

            DiscussionNode node = new DiscussionNode(editor.textNode);

            node.character = matchingCharacter;

            node.usePrevCamera = previousNode != null &&
                previousNode.character != null &&
                previousNode.character == matchingCharacter;

            VNTextData data = node.textData as VNTextData;

            if (data != null)
            {
                data.text = dialogueText;
            }

            if (!node.usePrevCamera && allPresets.Length > 0)
            {
                node.cameraPreset = allPresets[Random.Range(0, allPresets.Length)];
            }

            editor.discussionNodes.Add(node);
            previousNode = node;
        }

        EditorUtility.SetDirty(editor.segment);
        AssetDatabase.SaveAssets();

        editor.Repaint();
        editorWindow?.Close();
    }
}
