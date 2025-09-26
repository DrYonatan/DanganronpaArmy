using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class ConversationSettingsPopup : PopupWindowContent
{
   private ConversationSettings settings; // your reference
   private Vector2 scrollPosition;

   public ConversationSettingsPopup(ConversationSettings settings)
   {
      this.settings = settings;
   }

   public override Vector2 GetWindowSize()
   {
      return new Vector2(600, 400); // size of popup, tweak as needed
   }

   public override void OnGUI(Rect rect)
   {
      if (settings == null) return;

      EditorGUILayout.LabelField("Conversation Settings", EditorStyles.boldLabel);
      EditorGUILayout.Space(2);
      if (GUILayout.Button("Add Character"))
      {
         settings.characterPositions.Add(new CharacterPositionMapping());
      }
      scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, EditorStyles.helpBox);

      for (int i = settings.characterPositions.Count - 1; i >= 0; i--)
      {
         CharacterPositionMapping characterInfo = settings.characterPositions[i];

         EditorGUILayout.BeginVertical("box");
         EditorGUILayout.BeginHorizontal();

         if (GUILayout.Button("X", GUILayout.Width(20)))
         {
            settings.characterPositions.RemoveAt(i);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            continue;
         }

         // Character field
         EditorGUILayout.LabelField("Character", GUILayout.Width(60));
         characterInfo.character = (Character)EditorGUILayout.ObjectField(
            characterInfo.character,
            typeof(Character),
            false,
            GUILayout.Width(120)
         );

         EditorGUILayout.EndHorizontal();

         // Look direction dropdown
         characterInfo.position = (int)(CameraLookDirection)EditorGUILayout.EnumPopup(
            "Position",
            (CameraLookDirection)characterInfo.position
         );

         EditorGUILayout.EndVertical();
         EditorGUILayout.Space(2);
      }

      EditorGUILayout.EndScrollView();
   }
}

public class VNDialogueEditor : EditorWindow
{
   public List<DialogueNode> nodes;
   public ConversationSettings settings;
   public VNConversationSegment segment;
   public bool isSettingsEditable;
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
   
   // The Reason this receives both the segment and the nodes and settings is beacuse sometimes only the nodes and settings are sent
   public static void Open(List<DialogueNode> nodes, ConversationSettings settings, VNConversationSegment seg, bool isSettingsEditable)
   {
      var window = CreateInstance<VNDialogueEditor>();
      window.nodes = nodes;
      window.settings = settings;
      window.segment = seg;
      window.isSettingsEditable = isSettingsEditable;
      ShowEditor();
   }

   [OnOpenAsset]
   public static bool OnOpenAsset(int instanceID, int line)
   {
      var obj = EditorUtility.InstanceIDToObject(instanceID) as VNConversationSegment;
      if (obj != null)
      {
         Open(obj.nodes, obj.settings, obj,  true);
         return true;
      }

      return false;
   }

   void SetConversationSettings()
   {
      if (GUILayout.Button("Participating Characters", GUILayout.Width(150)))
      {
         PopupWindow.Show(
            new Rect(Event.current.mousePosition, Vector2.zero),
            new ConversationSettingsPopup(settings)
         );
      }
   }
   

   private void OnGUI()
   {
      if(segment != null)
         EditorUtility.SetDirty(segment);
      if (nodes == null)
      {
         return;
      }

      if (nodes.Count == 0)
      {
         AddNode(0);
      }
      
      foreach (var node in nodes)
      {
         if (node.drawNode == null)
         {
            if (node is VNChoiceNode)
            {
               node.drawNode = choiceNode;
            }

            else
            {
               node.drawNode = textNode;
            }
         }
      }
      
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
      if (nodes != null)
      {
         EditorGUILayout.Space(20);
         
         if (isSettingsEditable)
         {
            SetConversationSettings();
         }
         
         for (int i = 0; i < nodes.Count; i++)
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
               DrawNode(i, settings);
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
   
   void DrawNode(int id, ConversationSettings settings)
   {
      nodes[id].DrawNode(settings,window.position.width * 0.95f, window.position.height * 0.2f);
   }
   void AddNode(int index)
   {
      nodes.Insert(index, new DialogueNode(textNode));
   }

   void AddChoiceNode(int index)
   {
      nodes.Insert(index, new VNChoiceNode(choiceNode));
   }

   void RemoveNode(int index)
   {
      nodes.RemoveAt(index);
   }
}
