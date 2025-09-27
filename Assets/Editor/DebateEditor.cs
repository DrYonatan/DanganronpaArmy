using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEditor;
using UnityEngine;

public class DebateSettingsPopup : PopupWindowContent
{
   private DebateSegment container; // your reference
   public DebateSettingsPopup(DebateSegment container)
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

      EditorGUILayout.LabelField("Debate Settings", EditorStyles.boldLabel);
      GUILayout.BeginVertical();
      container.settings.audioClip = (AudioClip)EditorGUILayout.ObjectField("Music", container.settings.audioClip, typeof(AudioClip), false);
      for (int i = 0; i < container.settings.evidences.Length; i++)
      {
         container.settings.evidences[i] = (Evidence)EditorGUILayout.ObjectField("Evidence #" + i, container.settings.evidences[i], typeof(Evidence), false);
      }
      GUILayout.EndVertical();

   }
}

public class DebateEditor : EditorWindow
{
   public DebateSegment container;
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
      container = (DebateSegment)EditorGUILayout.ObjectField(container, typeof(DebateSegment), false, GUILayout.Width(200));

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
      EditorGUILayout.Space(20);
      GUILayout.BeginVertical(GUILayout.Width(300));
      
      if (GUILayout.Button("Debate Settings"))
      {
         var popup = new DebateSettingsPopup( container);
         PopupWindow.Show(new Rect(new Vector2(100, 50), Vector2.zero), popup);
         Event.current.Use(); // Optional: Consume the event
      }
      
      if (GUILayout.Button("Finish Nodes"))
      {
         ConversationEditor.Open(container.finishNodes, container.settings, null);
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
               DrawNode(i, container.settings);
            }
            
            if (GUILayout.Button("ADD NODE", GUILayout.Width(1400)))
            {
               AddNode(i+1);
            }
         }
      }
   }
   
   void DrawNode(int id, DebateSettings settings)
   {
      container.dialogueNodes[id].DrawNode(settings, window.position.width * 0.8f, window.position.height * 0.2f);
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
               DestroyImmediate(node.previewPivot.gameObject);

            if (node.previewTexture != null)
               node.previewTexture.Release();
         }
         foreach (var node in container.finishNodes)
         {
            if (node.previewCamera != null)
               DestroyImmediate(node.previewPivot.gameObject);

            if (node.previewTexture != null)
            {
               node.previewTexture.Release();
               DestroyImmediate(node.previewTexture);
            }
               
         }
      }
   }
}
