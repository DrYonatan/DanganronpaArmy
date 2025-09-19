using System;
using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
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

    #if UNITY_EDITOR
    [NonSerialized] public GameObject previewPivot;
    [NonSerialized] public Camera previewCamera;
    [NonSerialized] public RenderTexture previewTexture;
    #endif
    public DiscussionNode(DrawNode _drawNode) : base(_drawNode)
    {
        textData = new VNTextData();
    }

    public override IEnumerator Play()
    {
        CharacterStand characterStand = TrialDialogueManager.instance.characterStands.Find(stand => stand.character == character);
        if (!usePrevCamera)
        {
            TrialDialogueManager.instance.cameraController.TeleportToTarget(characterStand.transform,
                characterStand.heightPivot, positionOffset, rotationOffset, fovOffset);
            TrialDialogueManager.instance.effectController.Reset();
        }

        ((CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator)).ChangeFace(character.faceSprite);
        
        foreach (CameraEffect cameraEffect in cameraEffects)
        {
            TrialDialogueManager.instance.effectController.StartEffect(cameraEffect);
        }
        characterStand.state = expression;
        characterStand.SetSprite();

        yield return DialogueSystem.instance.Say(this);
    }
    
}