
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class ComicTextEditor : EditorWindow
{
    private ComicSegment segment;
    private ComicPage page;
    private ComicPanel panel;
    private Vector2 scrollPos;
    
    static EditorWindow window;


    static void ShowEditor()
    {
        window = GetWindow(typeof(ComicTextEditor));
        window.minSize = new Vector2(600, 800);
    }
    
    public static void Open(ComicSegment segment)
    {
        var window = CreateInstance<ComicTextEditor>();
        window.segment = segment;
        ShowEditor();
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID) as ComicSegment;
        if (obj != null)
        {
            Open(obj);
            return true;
        }
      
        return false;
    }

    private void OnGUI()
    {
        if (segment == null)
            return;
        
        EditorUtility.SetDirty(segment);
        
        if (page == null)
        {
            DrawMainMenu();
        }
        else if (page != null)
        {
            MarkDirty(page);

            if(panel == null)
               DrawSelectedPageMenu();
            else
            {
                MarkDirty(panel);
                DrawSelectedPanelMenu();
            }
        }
    }

    private void DrawMainMenu()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        foreach (ComicPage comicPage in segment.pages)
        {
            if (GUILayout.Button(comicPage.name))
            {
                page = comicPage;
            }
        }
        
        if (GUILayout.Button(segment.finalPage.name))
        {
            page = segment.finalPage;
        }
        GUILayout.EndScrollView();
    }

    private void DrawSelectedPageMenu()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        foreach (ComicPanel comicPanel in page.panels)
        {
            if (GUILayout.Button(comicPanel.name))
            {
                panel = comicPanel;
            }
        }
        GUILayout.EndScrollView();
    }

    private void DrawSelectedPanelMenu()
    {
        if(GUILayout.Button("Before Text"))
            VNDialogueEditor.Open(panel.textBeforePanel, null, null, false);

        var questionPanel = panel as ComicQuestionPanel;
        if (questionPanel is not null)
        {
            if(GUILayout.Button("Info Text"))
                VNDialogueEditor.Open(questionPanel.infoNodes, null, null, false);
        }
        
        if(GUILayout.Button("After Text"))
            VNDialogueEditor.Open(panel.textAfterPanel, null, null, false);
    }
    
    private void MarkDirty(UnityEngine.Object obj)
    {
        if (obj == null) return;
        EditorUtility.SetDirty(obj);
        if (PrefabUtility.IsPartOfPrefabInstance(obj))
            PrefabUtility.RecordPrefabInstancePropertyModifications(obj);
    }
}
