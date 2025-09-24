using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class ConversationEditor : EditorWindow
{
   [SerializeReference]
   public List<DiscussionNode> discussionNodes;
   public DrawNode textNode;
   public DiscussionChoiceNodeDraw choiceNode;
   protected Vector2 scrollPosition;
   
   static EditorWindow window;
   
   [MenuItem("Dialogue Editors/Court Discussion Editor")]
   static void ShowEditor()
   {
      window = GetWindow(typeof(ConversationEditor));
      window.minSize = new Vector2(600, 800);
   }

   public static void Open(List<DiscussionNode> discussionNodes)
   {
      var window = CreateInstance<ConversationEditor>();
      window.discussionNodes = discussionNodes;
      ShowEditor();
   }

   [OnOpenAsset]
   public static bool OnOpenAsset(int instanceID, int line)
   {
      var obj = EditorUtility.InstanceIDToObject(instanceID) as DiscussionSegment;
      if (obj != null)
      {
         Open(obj.discussionNodes);
         return true;
      }

      return false;
   }

   private void OnGUI()
   {
      // if (discussionNodes == null)
      // {
      //    segment = (DiscussionSegment)EditorGUILayout.ObjectField(segment, typeof(DiscussionSegment), false, GUILayout.Width(200));
      //
      //    if (segment != null)
      //    {
      //       discussionNodes = segment.discussionNodes;
      //       EditorUtility.SetDirty(segment);
      //    }
      // }

      if (discussionNodes.Count == 0)
      {
         AddNode(0);
      }
      
      foreach (var node in discussionNodes)
      {
         if (node.drawNode == null)
         {
            if (node is DiscussionChoiceNode)
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
      if (discussionNodes != null)
      {
         EditorGUILayout.Space(20);
         for (int i = 0; i < discussionNodes.Count; i++)
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
            GUILayout.Label("#" + (i + 1), indexLabelStyle, GUILayout.Width(25));
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
            
            GUILayout.Space(10);

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
      discussionNodes[id].DrawNode(window.position.width * 0.8f, window.position.height * 0.2f);
   }
   
   private void UserInput(Event e)
   {
    
   }
   void AddNode(int index)
   {
      discussionNodes.Insert(index, new DiscussionNode(textNode));
   }

   void AddChoiceNode(int index)
   {
      discussionNodes.Insert(index, new DiscussionChoiceNode(choiceNode));
   }

   void RemoveNode(int index)
   {
      if (discussionNodes[index].previewCamera != null)
         DestroyImmediate(discussionNodes[index].previewPivot.gameObject);

      if (discussionNodes[index].previewTexture != null)
      {
         discussionNodes[index].previewTexture.Release();
         DestroyImmediate(discussionNodes[index].previewTexture);
      }
      discussionNodes.RemoveAt(index);
   }
   private Rect windowRect;
   
   private void OnDisable()
   {
      if (discussionNodes != null)
      {
         foreach (var discussionNode in discussionNodes)
         {
            if (discussionNode.previewCamera != null)
               DestroyImmediate(discussionNode.previewPivot.gameObject);

            if (discussionNode.previewTexture != null)
            {
               discussionNode.previewTexture.Release();
               DestroyImmediate(discussionNode.previewTexture);
            }
               
         }
      }
   }
}
