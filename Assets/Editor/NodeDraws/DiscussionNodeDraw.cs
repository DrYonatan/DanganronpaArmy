using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CameraSettingsPopUp : PopupWindowContent
{
    private DiscussionNode node;
    private RenderTexture bigPreview;
    private int bigPreviewHeight = 360;
    private int bigPreviewWidth = 640;
    public CameraSettingsPopUp(DiscussionNode node)
    {
        this.node = node;
        bigPreview = new RenderTexture(bigPreviewWidth, bigPreviewHeight, 16);
        node.previewCamera.targetTexture = bigPreview;
    }

    public override Vector2 GetWindowSize() => new (650, 600);

    public override void OnGUI(Rect rect)
    {
        UpdatePreview(node);
        
        if (GUILayout.Button("X", GUILayout.Width(50)))
        {
            editorWindow.Close();
            node.previewCamera.targetTexture = node.previewTexture;
            bigPreview.Release();
        }
        GUILayout.Label(bigPreview, GUILayout.Width(bigPreviewWidth), GUILayout.Height(bigPreviewHeight));

        node.positionOffset = EditorGUILayout.Vector3Field("Position Offset", node.positionOffset);
        
        node.rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", node.rotationOffset);
        
        node.fovOffset = EditorGUILayout.FloatField("Fov Offset", node.fovOffset);
        
        ShowCameraEffects(ref node.cameraEffects, ref node);
    }
    
    private void UpdatePreview(TrialDialogueNode b)
    {
        if (b.previewCamera == null || b.character == null)
            return;
        b.previewPivot.transform.rotation = Quaternion.LookRotation(new Vector3(b.characterStand.transform.position.x, 0f, b.characterStand.transform.position.z));
        
        b.previewCamera.transform.localPosition = new Vector3(0f, b.characterStand.heightPivot.position.y, -1.65f) + b.positionOffset;
        b.previewCamera.transform.localRotation = Quaternion.Euler(b.rotationOffset);
        b.previewCamera.fieldOfView = 15f + b.fovOffset;
    
        b.previewCamera.Render();
    }
    
    private void ShowCameraEffects(ref List<CameraEffect> cameraEffects, ref DiscussionNode b)
    {
        for(int i = 0; i < cameraEffects.Count; i++)
        {
            GUILayout.BeginHorizontal();
            cameraEffects[i] = (CameraEffect)EditorGUILayout.ObjectField(cameraEffects[i], typeof(CameraEffect), false);
            if(GUILayout.Button("X", GUILayout.Width(20)))
            {
                cameraEffects.RemoveAt(i);
            }
            else
            {
                b.nodeRect.height += 20;  
            }
            
            GUILayout.EndHorizontal();
        }
        if(GUILayout.Button("Add camera effect"))
        {
            cameraEffects.Add(null);
        }
        b.nodeRect.height += 20;
    }
}

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Trial Discussion Node Draw")]
public class DiscussionNodeDraw : VNNodeDraw
{
    private const int previewWidth = 240;
    private const int previewHeight = 135;
    public override void DrawWindow(DialogueNode b)
    {
        DiscussionNode node = b as DiscussionNode;
        
        if (node == null)
        {
            EditorGUILayout.LabelField("Failed to cast node to DiscussionNode.");
            return;
        }
        
        GUILayout.BeginVertical();
        node.usePrevCamera = GUILayout.Toggle(node.usePrevCamera, "use previous camera");
        
        base.DrawWindow(b);
        GUILayout.EndVertical();
 
        node.characterStand = GameObject.Find($"Court/Characters/{node.character?.name}")?.GetComponent<CharacterStand>();
        
        SetupPreview(node);
        UpdatePreview(node);
   
    }

    protected override void ShowPreviewImage(DialogueNode node)
    {
        DiscussionNode discussionNode = node as DiscussionNode;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (discussionNode?.previewTexture != null)
        {
             GUILayout.Label(discussionNode.previewTexture, GUILayout.Width(previewWidth), GUILayout.Height(previewHeight));
             Rect lastRect = GUILayoutUtility.GetLastRect();

             if (Event.current.type == EventType.MouseDown && lastRect.Contains(Event.current.mousePosition))
             {
                 var popup = new CameraSettingsPopUp( discussionNode);
                 PopupWindow.Show(new Rect(new Vector2(100, 50), Vector2.zero), popup);
                 Event.current.Use(); // Optional: Consume the event
             }
        }
        else
        {
             GUILayout.Label("No Preview", GUILayout.Width(previewWidth), GUILayout.Height(previewHeight));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

    }
    
    private void SetupPreview(TrialDialogueNode b)
    {
        if (b.previewTexture == null)
        {
            b.previewTexture = new RenderTexture(previewWidth, previewHeight, 16);
        }
        if (b.previewCamera == null && b.characterStand != null)
        {
            GameObject pivot = new GameObject("PreviewPivot");
            pivot.transform.position = new Vector3(0f, 0f, 0f);
            GameObject camObj = new GameObject("NodePreviewCamera");
            camObj.transform.parent = pivot.transform;
            camObj.transform.localRotation = Quaternion.identity;
            
            b.previewPivot = pivot;
            b.previewCamera = camObj.AddComponent<Camera>();
            b.previewCamera.targetTexture = b.previewTexture;
            b.previewCamera.enabled = false;
        }
    }
    
    private void UpdatePreview(TrialDialogueNode b)
    {
        if (b.previewCamera == null || b.character == null)
            return;
        b.previewPivot.transform.rotation = Quaternion.LookRotation(new Vector3(b.characterStand.transform.position.x, 0f, b.characterStand.transform.position.z));
        
        b.previewCamera.transform.localPosition = new Vector3(0f, b.characterStand.heightPivot.position.y, -1.65f) + b.positionOffset;
        b.previewCamera.transform.localRotation = Quaternion.Euler(b.rotationOffset);
        b.previewCamera.fieldOfView = 15f + b.fovOffset;
    
        b.previewCamera.Render();
    }

}