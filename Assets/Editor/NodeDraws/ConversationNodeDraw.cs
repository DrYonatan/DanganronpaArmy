using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Trial Discussion Node Draw")]
public class ConversationNodeDraw : VNNodeDraw
{
    private const int previewWidth = 192;
    private const int previewHeight = 108;
    public override void DrawWindow(DialogueNode b)
    {
        b.nodeRect.height = 300;
        b.nodeRect.width = 200;
        base.DrawWindow(b);
        
        DiscussionNode node = b as DiscussionNode;
        
        if (node == null)
        {
            EditorGUILayout.LabelField("Failed to cast node to DiscussionNode.");
            return;
        }
        
        node.characterStand = GameObject.Find($"Court/Characters/{node.character?.name}")?.GetComponent<CharacterStand>();
        
        SetupPreview(node);
        UpdatePreview(node);
        
    }

    protected override void ShowPreviewImage(DialogueNode node)
    {
        DiscussionNode discussionNode = node as DiscussionNode;
        if (discussionNode?.previewTexture != null)
        {
             GUILayout.Label(discussionNode.previewTexture, GUILayout.Width(previewWidth), GUILayout.Height(previewHeight));
        }
        else
        {
             GUILayout.Label("No Preview", GUILayout.Width(previewWidth), GUILayout.Height(previewHeight));
        }
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