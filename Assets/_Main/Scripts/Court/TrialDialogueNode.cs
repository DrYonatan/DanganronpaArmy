using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class TrialDialogueNode : DialogueNode
{
    public CharacterCourt character;
    public CharacterStand characterStand;
    
    public List<CameraEffect> cameraEffects = new List<CameraEffect>();
    public float fovOffset;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    [NonSerialized] public GameObject previewPivot;
    [NonSerialized] public Camera previewCamera;
    [NonSerialized] public RenderTexture previewTexture;
    
    protected TrialDialogueNode(DrawNode _drawNode): base(_drawNode)
    {
        this.drawNode = _drawNode;
    }

    public void DrawNode()
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this);
        }
    }
    
    
}
