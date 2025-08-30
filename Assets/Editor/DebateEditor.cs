using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FinishNodesPopup : PopupWindowContent
{
   private List<DiscussionNode> nodes;

   public FinishNodesPopup(List<DiscussionNode> nodes)
   {
      this.nodes = nodes;
   }

   public override Vector2 GetWindowSize() => new(650, 600);

   public override void OnGUI(Rect rect)
   {
      foreach (DiscussionNode node in nodes)
      {
         node.DrawNode(650, 650);
      }
   }

}

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

      scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height((window.position.height * 0.8f)));

      Event e = Event.current;
      UserInput(e);

      BeginWindows();
      DrawEditor();
      EndWindows();

      GUILayout.EndScrollView();
      GUILayout.EndArea();
   }

   private void DrawDebateSettings()
   {
      GUILayout.BeginVertical(GUILayout.Width(300));
      container.audioClip = (AudioClip)EditorGUILayout.ObjectField("Music", container.audioClip, typeof(AudioClip), false);
      if (GUILayout.Button("Finish Nodes"))
      {
         var popup = new FinishNodesPopup( container.finishNodes);
         PopupWindow.Show(new Rect(new Vector2(100, 50), Vector2.zero), popup);
         Event.current.Use(); // Optional: Consume the event
      }
      GUILayout.EndVertical();
   }
   
   private void DrawEditor()
   {
      if (container != null)
      {
         DrawDebateSettings();
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
      container.dialogueNodes[id].DrawNode(window.position.width * 0.8f, window.position.height * 0.2f);
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
