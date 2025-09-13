using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VNDialogueEditor : EditorWindow
{
   public VNConversationSegment container;
   protected DialogueNode selectedNode;
   public DrawNode textNode;
   public ChoiceNodeDraw choiceNode;
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
         if (node.drawNode == null)
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
            GUILayout.BeginHorizontal();
            
            GUIStyle indexStyle = new GUIStyle();
            indexStyle.normal.background = Texture2D.whiteTexture;
            GUILayout.BeginVertical(indexStyle, GUILayout.Width(0.025f * window.position.width));
            GUILayout.Label("#" + (i+1));
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            
            GUIStyle boxStyle = new GUIStyle();
            boxStyle.normal.background = Texture2D.grayTexture;
            boxStyle.padding = new RectOffset(10, 10, 10, 10);
            boxStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.BeginVertical(boxStyle, GUILayout.Width(window.position.width * 0.95f));

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            bool button = GUILayout.Button("X", GUILayout.Width(50));
            GUI.backgroundColor = originalColor;
            GUILayout.EndHorizontal();
            
            if (button)
            {
               RemoveNode(i);
            }
            else
            {
               DrawNode(i);
            }

            int buttonsHeight = 50;
            GUILayout.BeginHorizontal(GUILayout.Height(buttonsHeight));
            
            if (GUILayout.Button("ADD NODE", GUILayout.Height(buttonsHeight)))
            {
               AddNode(i+1);
            }
            
            if (GUILayout.Button("ADD CHOICE NODE", GUILayout.Height(buttonsHeight)))
            {
               AddChoiceNode(i+1);
            }
            
            GUILayout.EndHorizontal();

            
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();

            
            GUILayout.Space(20);
         }
      }
   }
   
   void DrawNode(int id)
   {
      container.nodes[id].DrawNode(window.position.width * 0.95f, window.position.height * 0.2f);
   }
   public void AddNode(int index)
   {
      container.nodes.Insert(index, new DialogueNode(textNode));
   }

   public void AddChoiceNode(int index)
   {
      container.nodes.Insert(index, new ChoiceNode(choiceNode));
   }

   void RemoveNode(int index)
   {
      container.nodes.RemoveAt(index);
   }
}
