using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConversationEditor : EditorWindow
{
    public DiscussionSegment container;
    DiscussionNode selectedNode;
    public DrawNode textNode;
    Vector2 scrollPosition;
    Rect scrollAreaSize = new Rect(0, 0, 2000, 2000);
    Vector3 mousePosition;

    static EditorWindow window;

    [MenuItem("Dialogue Editors/Dialogue Editor")]
    static void ShowEditor()
    {
        window = GetWindow(typeof(ConversationEditor));
        window.minSize = new Vector2(800, 600);
    }
  
    private void OnGUI()
    {
        container = (DiscussionSegment)EditorGUILayout.ObjectField(container, typeof(DiscussionSegment), false, GUILayout.Width(200));

        if (container == null)
        {
            return;
        }
        
        foreach (var node in container.discussionNodes)
        {
            if (node.drawNode == null)
            {
                node.drawNode = textNode;
            }
        }

        EditorUtility.SetDirty(container);

        if (container.discussionNodes == null)
        {
            container.discussionNodes = new List<DiscussionNode>();
        }

        if (container.discussionNodes.Count > 0)
        {
            Rect nodeSize = container.discussionNodes[container.discussionNodes.Count - 1].nodeRect;
            scrollAreaSize.width = nodeSize.xMax + 10;
            scrollAreaSize.height = nodeSize.yMax + 10;

        }
        GUILayout.BeginArea(new Rect(0, 0, window.position.width, window.position.height));

        scrollPosition = GUI.BeginScrollView(new Rect(0, 0, window.position.width, window.position.height), scrollPosition, scrollAreaSize);

        Event e = Event.current;
        mousePosition = e.mousePosition;
        UserInput(e);
        DrawLines();

        BeginWindows();
        DrawEditor();
        EndWindows();

        GUI.EndScrollView();
        GUILayout.EndArea();
    }

    Rect windowRect;

    private void DrawLines()
    {
        if (container != null)
        {
            if (container.discussionNodes.Count > 1)
            {
                for (int i = 1; i < container.discussionNodes.Count; i++)
                {

                    ConnectLine(container.discussionNodes[i - 1].nodeRect, container.discussionNodes[i].nodeRect);

                }
            }


        }
    }

    private void ConnectLine(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + (start.height * 0.5f), 0f);
        Vector3 endPos = new Vector3(end.x + (end.width * 0.5f), end.y + (end.height * 0.5f), 0);

        ConnectLiveDraw(startPos, endPos);
    }

    private void ConnectLiveDraw(Vector3 start, Vector3 end)
    {
        Vector3 startTan = start + (Vector3.right * 50f);
        Vector3 endTan = end + (Vector3.left * 50f);

        Handles.DrawBezier(start, end, startTan, endTan, Color.black, null, 2);
    }

    private void UserInput(Event e)
    {
        if (container != null)
        {
            if (e.type == EventType.MouseDown && e.button == 1)
            {
                RightClick(e);
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                LeftClick(e);
            }
        }
    }

    private void LeftClick(Event e)
    {

    }

    private void RightClick(Event e)
    {
        selectedNode = null;
        CheckClickNode(e);

        if (selectedNode == null)
        {
            ContextMenu(e);
        }
        else
        {
            NodeContextMenu(e);
        }
    }

    private void NodeContextMenu(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Delete"), false, DeleteNode);
        menu.ShowAsContext();
        e.Use();
    }

    private void ContextMenu(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Create Node"), false, CreateNode);
        menu.ShowAsContext();
        e.Use();
    }

    void CreateNode()
    {
        container.discussionNodes.Add(new DiscussionNode(textNode));
    }
    
    void DeleteNode()
    {
        container.discussionNodes.Remove(selectedNode);
    }

    private void CheckClickNode(Event e)
    {
        for (int i = 0; i < container.discussionNodes.Count; i++)
        {
            if (container.discussionNodes[i].nodeRect.Contains(e.mousePosition))
            {
                selectedNode = container.discussionNodes[i];
                break;
            }
        }
    }




    float nodeOffset = 210f;
    private void DrawEditor()
    {
        if (container != null)
        {
            for (int i = 0; i < container.discussionNodes.Count; i++)
            {
                Rect rect = container.discussionNodes[i].nodeRect;
                rect.x = rect.width * i + 10 * i + 10 + nodeOffset;
                rect.y = 50;
                container.discussionNodes[i].nodeRect = rect;
                GUI.Window(i, container.discussionNodes[i].nodeRect, DrawNode, container.discussionNodes[i].title);
            }
        }

        
    }

    void DrawNode(int id)
    {
        container.discussionNodes[id].DrawNode();
    }
    
    private void OnDisable()
    {
        if (container != null && container.discussionNodes != null)
        {
            foreach (var node in container.discussionNodes)
            {
                if (node.previewCamera != null)
                    GameObject.DestroyImmediate(node.previewPivot.gameObject);

                if (node.previewTexture != null)
                    node.previewTexture.Release();
            }
        }
    }
}