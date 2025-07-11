using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConversationEditor : EditorWindow
{
    public ConversationSegment container;
    ConversationNode selectedNode;
    public DrawNode textNode;
    Vector2 scrollPosition;
    Rect scrollAreaSize = new Rect(0, 0, 2000, 2000);
    Vector3 mousePosition;

    static EditorWindow window;

    [MenuItem("Courtroom Editors/Dialogue Editor")]
    static void ShowEditor()
    {
        window = GetWindow(typeof(ConversationEditor));
        window.minSize = new Vector2(800, 600);
    }
  
    private void OnGUI()
    {
        container = (ConversationSegment)EditorGUILayout.ObjectField(container, typeof(ConversationSegment), false, GUILayout.Width(200));

        if (container == null)
        {
            return;
        }

        EditorUtility.SetDirty(container);

        if (container.conversationNodes == null)
        {
            container.conversationNodes = new List<ConversationNode>();
        }

        if (container.conversationNodes.Count > 0)
        {
            Rect nodeSize = container.conversationNodes[container.conversationNodes.Count - 1].nodeRect;
            scrollAreaSize.width = nodeSize.xMax + 10;
            scrollAreaSize.height = 400;

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


            if (container.conversationNodes.Count > 1)
            {
                for (int i = 1; i < container.conversationNodes.Count; i++)
                {

                    ConnectLine(container.conversationNodes[i - 1].nodeRect, container.conversationNodes[i].nodeRect);

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
        menu.AddItem(new GUIContent("Add Text Line"), false, AddTextLine);
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
        container.conversationNodes.Add(new ConversationNode(textNode));
    }

    void AddTextLine()
    {
        if (selectedNode.textLines == null)
        {
            selectedNode.textLines = new List<string>();
        }

        selectedNode.textLines.Add("");
    }
    

    void DeleteNode()
    {
        container.conversationNodes.Remove(selectedNode);
    }

    private void CheckClickNode(Event e)
    {
        for (int i = 0; i < container.conversationNodes.Count; i++)
        {
            if (container.conversationNodes[i].nodeRect.Contains(e.mousePosition))
            {
                selectedNode = container.conversationNodes[i];
                break;
            }
        }
    }




    float nodeOffset = 210f;
    private void DrawEditor()
    {
        if (container != null)
        {
            for (int i = 0; i < container.conversationNodes.Count; i++)
            {
                Rect rect = container.conversationNodes[i].nodeRect;
                rect.x = rect.width * i + 10 * i + 10 + nodeOffset;
                rect.y = 50;
                container.conversationNodes[i].nodeRect = rect;
                GUI.Window(i, container.conversationNodes[i].nodeRect, DrawNode, container.conversationNodes[i].title);
            }
        }

        
    }

    void DrawNode(int id)
    {
        container.conversationNodes[id].DrawNode();
    }
}