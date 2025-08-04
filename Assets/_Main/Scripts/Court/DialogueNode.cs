using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DialogueNode 
{
    public Rect nodeRect;
    public string title;

    public DrawNode drawNode;
    
    public string text;
    public CharacterCourt character;
    public CharacterStand characterStand;
    
    public List<CameraEffect> cameraEffects = new List<CameraEffect>();
    public float fovOffset;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    [NonSerialized] public GameObject previewPivot;
    [NonSerialized] public Camera previewCamera;
    [NonSerialized] public RenderTexture previewTexture;
    
    protected DialogueNode(DrawNode _drawNode)
    {
        drawNode = _drawNode;
    }

    public void DrawNode()
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this);
        }
    }
    
    
}
