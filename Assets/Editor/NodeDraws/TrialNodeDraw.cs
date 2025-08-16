// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// public class TrialNodeDraw : VNNodeDraw
// {
//     
//     private const int previewWidth = 192;
//     private const int previewHeight = 108;
//     public override void DrawWindow(DialogueNode b, float wi)
//     {
//         base.DrawWindow(b);
//         // TrialDialogueNode node = (TrialDialogueNode)b;
//         // node.character = (CharacterCourt)EditorGUILayout.ObjectField(node.character, typeof(CharacterCourt), false);
//         // node.characterStand = GameObject.Find($"Court/Characters/{node.character?.name}")?.GetComponent<CharacterStand>();
//         //
//         // if (node.previewTexture != null)
//         // {
//         //     GUILayout.Label(node.previewTexture, GUILayout.Width(previewWidth), GUILayout.Height(previewHeight));
//         // }
//         // else
//         // {
//         //     GUILayout.Label("No Preview", GUILayout.Width(previewWidth), GUILayout.Height(previewHeight));
//         // }
//         //
//         // node.positionOffset = EditorGUILayout.Vector3Field("Position Offset", node.positionOffset);
//         //
//         // node.rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", node.rotationOffset);
//         //
//         // node.fovOffset = EditorGUILayout.FloatField("Fov Offset", node.fovOffset);
//         //
//         // ShowCameraEffects(ref node.cameraEffects, ref node);
//         // SetupPreview(node);
//         // UpdatePreview(node);
//     }
//     
//     private void ShowCameraEffects(ref List<CameraEffect> cameraEffects, ref TrialDialogueNode b)
//     {
//         for(int i = 0; i < cameraEffects.Count; i++)
//         {
//             GUILayout.BeginHorizontal();
//             cameraEffects[i] = (CameraEffect)EditorGUILayout.ObjectField(cameraEffects[i], typeof(CameraEffect), false);
//             if(GUILayout.Button("X", GUILayout.Width(20)))
//             {
//                 cameraEffects.RemoveAt(i);
//             }
//             else
//             {
//                 b.nodeRect.height += 20;  
//             }
//             
//             GUILayout.EndHorizontal();
//         }
//         if(GUILayout.Button("Add camera effect"))
//         {
//             cameraEffects.Add(null);
//         }
//         b.nodeRect.height += 20;
//     }
//     
//     private void SetupPreview(TrialDialogueNode b)
//     {
//         if (b.previewTexture == null)
//         {
//             b.previewTexture = new RenderTexture(previewWidth, previewHeight, 16);
//         }
//
//         if (b.previewCamera == null && b.characterStand != null)
//         {
//             GameObject pivot = new GameObject("PreviewPivot");
//             pivot.transform.position = new Vector3(0f, 0f, 0f);
//             GameObject camObj = new GameObject("NodePreviewCamera");
//             camObj.transform.parent = pivot.transform;
//             camObj.transform.localRotation = Quaternion.identity;
//             
//             pivot.hideFlags = HideFlags.HideAndDontSave;
//             b.previewPivot = pivot;
//             b.previewCamera = camObj.AddComponent<Camera>();
//             b.previewCamera.targetTexture = b.previewTexture;
//             b.previewCamera.enabled = false;
//         }
//     }
//     
//     private void UpdatePreview(TrialDialogueNode b)
//     {
//         if (b.previewCamera == null || b.character == null)
//             return;
//         b.previewPivot.transform.rotation = Quaternion.LookRotation(new Vector3(b.characterStand.transform.position.x, 0f, b.characterStand.transform.position.z));
//         
//         b.previewCamera.transform.localPosition = new Vector3(0f, b.characterStand.heightPivot.position.y, -1.65f) + b.positionOffset;
//         b.previewCamera.transform.localRotation = Quaternion.Euler(b.rotationOffset);
//         b.previewCamera.fieldOfView = 15f + b.fovOffset;
//     
//         b.previewCamera.Render();
//     }
//     
//     
// }
