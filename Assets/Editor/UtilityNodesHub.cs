using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class UtilityNodesHub : EditorWindow
{
    public UtilityNodesCollection utilityNodesCollection;
    static EditorWindow window;

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

        for (int i = 0; i < utilityNodesCollection.characterDefaultWrongNodes.Count; i++)
        {
            GUILayout.BeginHorizontal();
            Character character = utilityNodesCollection.characterDefaultWrongNodes[i].character;
            character = (Character)EditorGUILayout.ObjectField(character, typeof(DebateSegment), false,
                GUILayout.Width(200));
            if (GUILayout.Button("Edit Nodes"))
            {
                ConversationEditor.Open(utilityNodesCollection.characterDefaultWrongNodes[i].nodes, null, null);
            }
            GUILayout.EndHorizontal();
        }
}
}
