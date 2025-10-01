using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class UtilityNodesHub : EditorWindow
{
    public UtilityNodesCollection utilityNodesCollection;
    static EditorWindow window;
    private Vector2 characterDefaultWrongNodesScrollPosition;

    static void ShowEditor()
    {
        window = GetWindow(typeof(UtilityNodesHub));
    }
    static void Open(UtilityNodesCollection nodesCollection)
    {
        var window = CreateInstance<UtilityNodesHub>();
        window.utilityNodesCollection = nodesCollection;
        ShowEditor();
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
        
        GUILayout.BeginHorizontal();
        DrawCharactersDefaultNodes();
        DrawWrongAnswerNode();
        GUILayout.EndHorizontal();

    }

    private void DrawCharactersDefaultNodes()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Character Default Wrong Nodes");
        if (GUILayout.Button("Add",  GUILayout.Width(100)))
        {
            utilityNodesCollection.characterDefaultWrongNodes.Add(new CharacterDefaultWrongNodes());
        }

        characterDefaultWrongNodesScrollPosition = GUILayout.BeginScrollView(characterDefaultWrongNodesScrollPosition, GUILayout.Height(300), GUILayout.Width(400));
        for (int i = 0; i < utilityNodesCollection.characterDefaultWrongNodes.Count; i++)
        {
            GUILayout.BeginHorizontal(GUILayout.Height(50));
            
            utilityNodesCollection.characterDefaultWrongNodes[i].character = (Character)EditorGUILayout.ObjectField(utilityNodesCollection.characterDefaultWrongNodes[i].character, typeof(Character), false,
                GUILayout.Width(200));
            if (GUILayout.Button("Edit Nodes", GUILayout.Width(100)))
            {
                ConversationEditor.Open(utilityNodesCollection.characterDefaultWrongNodes[i].nodes, null, null);
            }

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                utilityNodesCollection.characterDefaultWrongNodes.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void DrawWrongAnswerNode()
    {
        GUILayout.Label("Wrong Answer Node");
        if (GUILayout.Button("Open", GUILayout.Width(100)))
        {
            ConversationEditor.Open(utilityNodesCollection.wrongAnswer, null, null);
        }
    }
}
