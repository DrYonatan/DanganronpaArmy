using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class VNDialogueEditor : EditorWindow
{
   public VNConversationSegment container;
   protected DialogueNode selectedNode;
   public DrawNode textNode;
   protected Vector2 scrollPosition;
   protected Rect scrollAreaSize = new Rect(0, 0, 2000, 2000);
   public float nodeSpacing = 10;
   
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
         AddNode(0);
      }
      
      foreach (var node in container.nodes)
      {
         if (node.drawNode != null)
         {
            node.drawNode = textNode;
         }
      }
      EditorUtility.SetDirty(container);
      
      GUILayout.BeginArea(new Rect(0, 0, window.position.width, window.position.height));

      scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height((window.position.height)));

      Event e = Event.current;
      UserInput(e);

      BeginWindows();
      DrawEditor();
      EndWindows();

      GUILayout.EndScrollView();
      GUILayout.EndArea();
   }
   
   private void DrawEditor()
   {
      if (container != null)
      {
         GUILayout.Box("", GUILayout.Height(150), GUILayout.Width(300));
         for (int i = 0; i < container.nodes.Count; i++)
         {
            if (GUILayout.Button("X", GUILayout.Width(50)))
            {
               RemoveNode(i);
            }
            else
            {
               DrawNode(i);
            }
            
            if (GUILayout.Button("ADD NODE", GUILayout.Width(1400)))
            {
               AddNode(i+1);
            }
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
   void AddNode(int index)
   {
      container.nodes.Insert(index, new DialogueNode(textNode));
   }

   void RemoveNode(int index)
   {
      container.nodes.RemoveAt(index);
   }
   private Rect windowRect;
}
