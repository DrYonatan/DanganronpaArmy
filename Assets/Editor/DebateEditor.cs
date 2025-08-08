using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DebateEditor : EditorWindow
{
   public Stage container;
   protected DialogueNode selectedNode;
   public DrawNode textNode;
   protected Vector2 scrollPosition;
   
   static EditorWindow window;
   
   [MenuItem("Dialogue Editors/Debate Editor")]
   static void ShowEditor()
   {
      window = GetWindow(typeof(DebateEditor));
      window.minSize = new Vector2(600, 800);
   }

   private void OnGUI()
   {
      container = (Stage)EditorGUILayout.ObjectField(container, typeof(Stage), false, GUILayout.Width(200));

      if (container == null)
      {
         return;
      }
      
      if (container.dialogueNodes == null)
      {
         container.dialogueNodes = new List<DebateNode>();
      }

      if (container.dialogueNodes.Count == 0)
      {
         AddNode(0);
      }
      
      foreach (var node in container.dialogueNodes)
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
         for (int i = 0; i < container.dialogueNodes.Count; i++)
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
      container.dialogueNodes[id].DrawNode();
   }
   
   private void UserInput(Event e)
   {
      if (container != null)
      {
         
      }
   }
   void AddNode(int index)
   {
      container.dialogueNodes.Insert(index, new DebateNode(textNode));
   }

   void RemoveNode(int index)
   {
      container.dialogueNodes.RemoveAt(index);
   }
   private Rect windowRect;
   
   private void OnDisable()
   {
      if (container != null && container.dialogueNodes != null)
      {
         foreach (var node in container.dialogueNodes)
         {
            if (node.previewCamera != null)
               GameObject.DestroyImmediate(node.previewPivot.gameObject);

            if (node.previewTexture != null)
               node.previewTexture.Release();
         }
      }
   }
}
