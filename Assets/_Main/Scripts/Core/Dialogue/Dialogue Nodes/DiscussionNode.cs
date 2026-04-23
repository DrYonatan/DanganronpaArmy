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
    }

    public override IEnumerator Play()
    {
        VNTextData data = textData as VNTextData;
        
        if(data == null)
            yield break;

        if (character == null)
            yield break;

        yield return DialogueSystem.instance.RunBeforeCommands(data.commands);
        
        CharacterStand characterStand = TrialManager.instance.characterStands.Find(stand => stand.character == character);

        if (!usePrevCamera)
        {
            TrialDialogueManager.instance.effectController.Reset();
        }
        
        if (characterStand != null)
        {
            HandleVisibleCharacter(characterStand);
        }

        else
        {
            HandleNonVisibleCharacter();
        }

        
        foreach (CameraEffect cameraEffect in cameraEffects)
        {
            TrialDialogueManager.instance.effectController.StartEffect(cameraEffect);
        }

        yield return DialogueSystem.instance.Say(this);
    }

    private void HandleVisibleCharacter(CharacterStand stand)
    {
        if (!usePrevCamera)
        {
            TrialDialogueManager.instance.cameraController.TeleportToTarget(stand.transform, stand.heightPivot, positionOffset, rotationOffset, fovOffset);
        }
        
        stand.SetSprite(character.emotions[expressionIndex]);
            
        CourtTextBoxAnimator animator = (CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator);

        if (!animator.characterFace.isVisible && !GameLoop.instance.isActive)
        {
            animator.FaceAppear();
        }
        animator.ChangeFace(character.faceSprite);
    }

    private void HandleNonVisibleCharacter()
    {
        CourtTextBoxAnimator animator = (CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator);

        if(animator.characterFace.isVisible)
           animator.characterFace.DiscussionFaceContainerDisappear(animator.duration);
    }
    
}