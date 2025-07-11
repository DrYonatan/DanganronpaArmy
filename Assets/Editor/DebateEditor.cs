using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEditor;
using UnityEngine;


public class DebateEditor : EditorWindow
{
    public Stage container;
    Vector3 mousePosition;
    DebateNode selectedNode;
    public DrawNode textNode;
    Vector2 scrollPosition;
    Rect scrollAreaSize = new Rect(0, 0, 2000, 2000);


    static EditorWindow window;

    [MenuItem("Courtroom Editors/Debate Editor")]
    static void ShowEditor()
    {
        window = EditorWindow.GetWindow(typeof(DebateEditor));
        window.minSize = new Vector2(800, 600);
    }
    
    private void OnGUI()
    {
        container = (Stage)EditorGUILayout.ObjectField(container, typeof(Stage), false, GUILayout.Width(200));

        if (container == null)
        {
            return;
        }

        EditorUtility.SetDirty(container);

        if (container.dialogueNodes == null)
        {
            container.dialogueNodes = new List<DebateNode>();
        }

        if (container.dialogueNodes.Count > 0)
        {
            Rect nodeSize = container.dialogueNodes[container.dialogueNodes.Count - 1].nodeRect;
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
        DrawStageSettingsWindow();
        EndWindows();

        GUI.EndScrollView();
        GUILayout.EndArea();
    }

    Rect windowRect;
    private void DrawStageSettingsWindow()
    {
        if(container != null)
        {
            windowRect = new Rect(
                5f, 30f, 200f, 200f
                );
        }
        GUI.Window(-1, windowRect, DrawStageSetting, "Stage Settings");
    }


    private void DrawStageSetting(int id)
    {
        if (container.evidences == null)
        {
            container.evidences = new Evidence[5];
        }
        if (container.evidences.Length != 5)
        {
            container.evidences = new Evidence[5];
        }

        container.audioClip = (AudioClip)EditorGUILayout.ObjectField(container.audioClip, typeof(AudioClip), false);
        for (int i = 0; i < container.evidences.Length; i++)
        {
            container.evidences[i] = (Evidence)EditorGUILayout.ObjectField(container.evidences[i], typeof(Evidence), false);
        }
    }


    private void DrawLines()
    {
        if (container != null)
        {


            if (container.dialogueNodes.Count > 1)
            {
                for (int i = 1; i < container.dialogueNodes.Count; i++)
                {

                    ConnectLine(container.dialogueNodes[i - 1].nodeRect, container.dialogueNodes[i].nodeRect);

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
        container.dialogueNodes.Add(new DebateNode(textNode));
    }

    void AddTextLine()
    {
        if (selectedNode.textLines == null)
        {
            selectedNode.textLines = new List<TextLine>();
        }
        selectedNode.textLines.Add(new TextLine());
    }

    void DeleteNode()
    {
        container.dialogueNodes.Remove(selectedNode);
    }

    private void CheckClickNode(Event e)
    {
        for (int i = 0; i < container.dialogueNodes.Count; i++)
        {
            if (container.dialogueNodes[i].nodeRect.Contains(e.mousePosition))
            {
                selectedNode = container.dialogueNodes[i];
                break;
            }
        }
    }




    float nodeOffset = 210f;
    private void DrawEditor()
    {
        
        if (container != null)
        {
            for (int i = 0; i < container.dialogueNodes.Count; i++)
            {
                Rect rect = container.dialogueNodes[i].nodeRect;
                rect.x = rect.width * i + 10 * i + 10 + nodeOffset;
                rect.y = 50;
                container.dialogueNodes[i].nodeRect = rect;
                GUI.Window(i, container.dialogueNodes[i].nodeRect, DrawNode, container.dialogueNodes[i].title);
            }
        }

        
    }

    void DrawNode(int id)
    {
        container.dialogueNodes[id].DrawNode();
    }
}