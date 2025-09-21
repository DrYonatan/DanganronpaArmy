using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConversationSettingsPopup : PopupWindowContent
{
   private VNConversationSegment container; // your reference
   private Vector2 scrollPosition;

   public ConversationSettingsPopup(VNConversationSegment container)
   {
      this.container = container;
   }

   public override Vector2 GetWindowSize()
   {
      return new Vector2(600, 400); // size of popup, tweak as needed
   }

   public override void OnGUI(Rect rect)
   {
      if (container == null) return;

      EditorGUILayout.LabelField("Conversation Settings", EditorStyles.boldLabel);
      EditorGUILayout.Space(2);
      if (GUILayout.Button("Add Character"))
      {
         container.CharacterInfos.Add(new VNCharacterInfo());
      }
      scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, EditorStyles.helpBox);

      for (int i = container.CharacterInfos.Count - 1; i >= 0; i--)
      {
         VNCharacterInfo characterInfo = container.CharacterInfos[i];

         EditorGUILayout.BeginVertical("box");
         EditorGUILayout.BeginHorizontal();

         if (GUILayout.Button("X", GUILayout.Width(20)))
         {
            container.CharacterInfos.RemoveAt(i);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            continue;
         }

         // Character field
         EditorGUILayout.LabelField("Character", GUILayout.Width(60));
         characterInfo.Character = (CharacterCourt)EditorGUILayout.ObjectField(
            characterInfo.Character,
            typeof(CharacterCourt),
            false,
            GUILayout.Width(120)
         );

         EditorGUILayout.EndHorizontal();

         // Look direction dropdown
         characterInfo.LookDirection = (CameraLookDirection)EditorGUILayout.EnumPopup(
            "Position",
            characterInfo.LookDirection
         );

         EditorGUILayout.EndVertical();
         EditorGUILayout.Space(2);
      }

      EditorGUILayout.EndScrollView();
   }
}

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
   
   public static void Open(List<DialogueNode> nodes)
   {
      var window = CreateInstance<VNDialogueEditor>();
      window.container.nodes = nodes;
      ShowEditor();
   }

   void SetConversationSettings()
   {
      if (GUILayout.Button("Character Settings", GUILayout.Width(150)))
      {
         PopupWindow.Show(
            new Rect(Event.current.mousePosition, Vector2.zero),
            new ConversationSettingsPopup(container)
         );
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
         EditorGUILayout.Space(20);
         SetConversationSettings();
         for (int i = 0; i < container.nodes.Count; i++)
         { 
            GUILayout.BeginHorizontal();
            GUIStyle boxStyle = new GUIStyle();
            boxStyle.normal.background = Texture2D.grayTexture;
            boxStyle.padding = new RectOffset(10, 10, 10, 10);
            boxStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.BeginVertical(boxStyle, GUILayout.Width(window.position.width * 0.95f));

            GUILayout.BeginHorizontal();
            GUIStyle indexLabelStyle = new GUIStyle();
            indexLabelStyle.normal.background = Texture2D.whiteTexture;
            indexLabelStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("#" + (i+1), indexLabelStyle, GUILayout.Width(25));
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
      container.nodes.Insert(index, new VNChoiceNode(choiceNode));
   }

   void RemoveNode(int index)
   {
      container.nodes.RemoveAt(index);
   }
}
