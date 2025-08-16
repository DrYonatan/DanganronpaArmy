using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VNDialogueEditor : EditorWindow
{
   public VNConversationSegment container;
   protected DialogueNode selectedNode;
   public DrawNode textNode;
   Vector2 scrollPosition;
   
   static EditorWindow window;
   
   [MenuItem("Dialogue Editors/VNDialogueEditor")]
   static void ShowEditor()
   {
      window = GetWindow(typeof(VNDialogueEditor));
      window.minSize = new Vector2(600, 800);
   }

   void SetConversationSettings()
   {
      if (container != null)
      {
         foreach(VNCharacterInfo characterInfo in container.CharacterInfos)
         {
            characterInfo.Character = (CharacterCourt)EditorGUILayout.ObjectField(characterInfo.Character, typeof(CharacterCourt), false, GUILayout.Width(50));
            characterInfo.LookDirection = (CameraLookDirection)EditorGUILayout.EnumPopup(
               "Character Position",
               characterInfo.LookDirection
            );
         }
      }
      
   }

   void SetContainer()
   {
      container = (VNConversationSegment)EditorGUILayout.ObjectField(container, typeof(VNConversationSegment), false, GUILayout.Width(200));
   }

   void SetNewList()
   {
      container.nodes = new List<DialogueNode>();
   }

   private void OnGUI()
   {
      SetContainer();
      SetConversationSettings();
      
      if (container == null)
      {
         return;
      }
      
      if (container.nodes == null)
      {
         SetNewList();
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

      scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height((window.position.height * 0.8f)));

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
      container.nodes[id].DrawNode(window.position.width * 0.8f, window.position.height * 0.2f);
   }
   public void AddNode(int index)
   {
      container.nodes.Insert(index, new DialogueNode(textNode));
   }

   void RemoveNode(int index)
   {
      container.nodes.RemoveAt(index);
   }
}
