using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class VNDialogueEditor : EditorWindow
{
   public VNConversationSegment container;
   DialogueNode selectedNode;
   public DrawNode textNode;
   Vector2 scrollPosition;
   Rect scrollAreaSize = new Rect(0, 0, 2000, 2000);
   
   static EditorWindow window;
   
   [MenuItem("Dialogue Editors/VNDialogueEditor")]
   static void ShowEditor()
   {
      window = GetWindow(typeof(VNDialogueEditor));
      window.minSize = new Vector2(600, 800);
   }

   private void OnGUI()
   {
      container = (VNConversationSegment)EditorGUILayout.ObjectField(container, typeof(VNConversationSegment), false, GUILayout.Width(200));

      if (container == null)
      {
         return;
      }
      
      if (container.nodes == null)
      {
         container.nodes = new List<DialogueNode>();
      }

      if (container.nodes.Count == 0)
      {
         CreateNode();
      }
      
      foreach (var node in container.nodes)
      {
         if (node.drawNode != null)
         {
            node.drawNode = textNode;
         }
      }
      EditorUtility.SetDirty(container);

      if (container.nodes.Count > 0)
      {
         Rect nodeSize = container.nodes[container.nodes.Count - 1].nodeRect;
         scrollAreaSize.width = nodeSize.xMax + 10;
         scrollAreaSize.height = nodeSize.yMax + 10;

      }
      GUILayout.BeginArea(new Rect(0, 0, window.position.width, window.position.height));

      scrollPosition = GUI.BeginScrollView(new Rect(0, 0, window.position.width, window.position.height), scrollPosition, scrollAreaSize);

      Event e = Event.current;
      UserInput(e);

      BeginWindows();
      DrawEditor();
      EndWindows();

      GUI.EndScrollView();
      GUILayout.EndArea();
   }
   
   float nodeOffset = 210f;
   private void DrawEditor()
   {
      if (container != null)
      {
         GUILayout.Box("", GUILayout.Height(150), GUILayout.Width(300));
         for (int i = 0; i < container.nodes.Count; i++)
         {
            DrawNode(i);
            GUILayout.Button("ADD NODE", GUILayout.Width(1400));
         }
      }
   }
   
   void DrawNode(int id)
   {
      container.nodes[id].DrawNode();
   }
   
   private void UserInput(Event e)
   {
      if (container != null)
      {
         
      }
   }
   void CreateNode()
   {
      container.nodes.Add(new DialogueNode(textNode));
   }
   private Rect windowRect;
}
