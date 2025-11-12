using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class UtilityNodesHub : EditorWindow
{
    public UtilityNodesCollection utilityNodesCollection;
    private Vector2 characterDefaultWrongNodesScrollPosition;

    [MenuItem("Tools/Utility Nodes Hub")]
    static void ShowEditor()
    {
        GetWindow<UtilityNodesHub>("Utility Nodes Hub");
    }

    static void Open(UtilityNodesCollection nodesCollection)
    {
        var window = GetWindow<UtilityNodesHub>("Utility Nodes Hub");
        window.utilityNodesCollection = nodesCollection;
        window.Show();
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID) as UtilityNodesCollection;
        if (obj != null)
        {
            Open(obj);
            return true;
        }

        return false;
    }

    private void OnGUI()
    {
        if (utilityNodesCollection != null)
            EditorUtility.SetDirty(utilityNodesCollection);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Utility Nodes Hub", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        DrawCharactersDefaultNodes();

        EditorGUILayout.BeginVertical("box", GUILayout.Width(220));
        DrawGenericNodesOpen("Discussion Wrong Answer Nodes",
            () => ConversationEditor.Open(utilityNodesCollection.wrongAnswer, null, null));
        DrawGenericNodesOpen("Debate Wrong Evidence Nodes",
            () => ConversationEditor.Open(utilityNodesCollection.debateWrongEvidence, null, null));
        DrawGenericNodesOpen("Get Out of Room Nodes",
            () => VNDialogueEditor.Open(utilityNodesCollection.getOutOfRoom, null, null, false));
        DrawGenericNodesOpen("Game Over Nodes",
            () => ConversationEditor.Open(utilityNodesCollection.gameOverNodes, null, null));
        DrawGenericNodesOpen("Wrong Comic Nodes",
            () => VNDialogueEditor.Open(utilityNodesCollection.wrongComicNodes, null, null, false));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawCharactersDefaultNodes()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(420));
        EditorGUILayout.LabelField("Character Default Wrong Nodes", EditorStyles.boldLabel);

        if (GUILayout.Button("➕ Add Character Nodes", GUILayout.Height(25)))
        {
            utilityNodesCollection.characterDefaultWrongNodes.Add(new CharacterDefaultWrongNodes());
        }

        EditorGUILayout.Space();

        characterDefaultWrongNodesScrollPosition = EditorGUILayout.BeginScrollView(
            characterDefaultWrongNodesScrollPosition,
            GUILayout.Height(320));

        for (int i = 0; i < utilityNodesCollection.characterDefaultWrongNodes.Count; i++)
        {
            var entry = utilityNodesCollection.characterDefaultWrongNodes[i];

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();

            entry.character = (Character)EditorGUILayout.ObjectField(
                entry.character, typeof(Character), false, GUILayout.Width(220));

            if (GUILayout.Button("Edit Nodes", GUILayout.Width(100)))
            {
                ConversationEditor.Open(entry.nodes, null, null);
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                utilityNodesCollection.characterDefaultWrongNodes.RemoveAt(i);
                GUI.backgroundColor = Color.white;
                break;
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawGenericNodesOpen(string label, Action onOpen)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(150));
        if (GUILayout.Button("Open", GUILayout.Width(60)))
        {
            onOpen();
        }

        EditorGUILayout.EndHorizontal();
    }
}