using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ConversationEditor : EditorWindow
{
   public DiscussionSegment container;
   protected DialogueNode selectedNode;
   public DrawNode textNode;
   protected Vector2 scrollPosition;
   
   static EditorWindow window;
   
   [MenuItem("Dialogue Editors/Court Discussion Editor")]
   static void ShowEditor()
   {
      window = GetWindow(typeof(ConversationEditor));
      window.minSize = new Vector2(600, 800);
   }

   private void OnGUI()
   {
      container = (DiscussionSegment)EditorGUILayout.ObjectField(container, typeof(DiscussionSegment), false, GUILayout.Width(200));

      if (container == null)
      {
         return;
      }
      
      if (container.discussionNodes == null)
      {
         container.discussionNodes = new List<DiscussionNode>();
      }

      if (container.discussionNodes.Count == 0)
      {
         AddNode(0);
      }
      
      foreach (var node in container.discussionNodes)
      {
         if (node.drawNode != null)
         {
            node.drawNode = textNode;
         }
      }
      EditorUtility.SetDirty(container);
      
      GUILayout.BeginArea(new Rect(0, 0, window.position.width, window.position.height));

      scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height((window.position.height * 0.8f)));

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
         for (int i = 0; i < container.discussionNodes.Count; i++)
         {
            if (GUILayout.Button("X", GUILayout.Width(50)))
            {
               RemoveNode(i);
            }
            else
            {
               DrawNode(i);
            }
            
            if (GUILayout.Button("ADD NODE"))
            {
               AddNode(i+1);
            }
         }
      }
   }
   
   void DrawNode(int id)
   {
      container.discussionNodes[id].DrawNode(window.position.width * 0.8f, window.position.height * 0.2f);
   }
   
   private void UserInput(Event e)
   {
      if (container != null)
      {
         
      }
   }
   void AddNode(int index)
   {
      container.discussionNodes.Insert(index, new DiscussionNode(textNode));
   }

   void RemoveNode(int index)
   {
      container.discussionNodes.RemoveAt(index);
   }
   private Rect windowRect;
   
   private void OnDisable()
   {
      if (container != null && container.discussionNodes != null)
      {
         foreach (var node in container.discussionNodes)
         {
            DiscussionNode discussionNode = node as DiscussionNode;
            if (discussionNode.previewCamera != null)
               GameObject.DestroyImmediate(discussionNode.previewPivot.gameObject);

            if (discussionNode.previewTexture != null)
               discussionNode.previewTexture.Release();
         }
      }
   }
}
