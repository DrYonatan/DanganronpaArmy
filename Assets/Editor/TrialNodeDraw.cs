using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrialNodeDraw : DrawNode
{
    
    private const int previewWidth = 192;
    private const int previewHeight = 108;
    public override void DrawWindow(DialogueNode b)
    {
        b.character = (CharacterCourt)EditorGUILayout.ObjectField(b.character, typeof(CharacterCourt), false);
        b.characterStand = GameObject.Find($"Court/Characters/{b.character?.name}")?.GetComponent<CharacterStand>();
        
        if (b.previewTexture != null)
        {
            GUILayout.Label(b.previewTexture, GUILayout.Width(previewWidth), GUILayout.Height(previewHeight));
        }
        else
        {
            GUILayout.Label("No Preview", GUILayout.Width(previewWidth), GUILayout.Height(previewHeight));
        }
        
        b.positionOffset = EditorGUILayout.Vector3Field("Position Offset", b.positionOffset);
        
        b.rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", b.rotationOffset);
        
        b.fovOffset = EditorGUILayout.FloatField("Fov Offset", b.fovOffset);


        ShowCameraEffect(ref b.cameraEffects, ref b);
        SetupPreview(b);
        UpdatePreview(b);
    }
    
    private void ShowCameraEffect(ref List<CameraEffect> cameraEffects, ref DialogueNode b)
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
    
    private void SetupPreview(DialogueNode b)
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
            
            pivot.hideFlags = HideFlags.HideAndDontSave;
            b.previewPivot = pivot;
            b.previewCamera = camObj.AddComponent<Camera>();
            b.previewCamera.targetTexture = b.previewTexture;
            b.previewCamera.enabled = false;
        }
    }
    
    private void UpdatePreview(DialogueNode b)
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
