using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiscussionNode : DialogueNode
{
    public bool usePrevCamera;
    public CharacterStand characterStand;
    
    public List<CameraEffect> cameraEffects = new List<CameraEffect>();
    public float fovOffset;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    [NonSerialized] public GameObject previewPivot;
    [NonSerialized] public Camera previewCamera;
    [NonSerialized] public RenderTexture previewTexture;
    
    public DiscussionNode(DrawNode _drawNode) : base(_drawNode)
    {
        textData = new VNTextData();
    }
}