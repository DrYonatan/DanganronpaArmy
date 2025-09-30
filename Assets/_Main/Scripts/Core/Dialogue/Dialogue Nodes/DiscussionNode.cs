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
    
    IEnumerator PlayConversationNode(DiscussionNode node)
    {
        CharacterStand characterStand = TrialManager.instance.characterStands.Find(stand => stand.character == node.character);
        if (!node.usePrevCamera)
        {
            TrialDialogueManager.instance.cameraController.TeleportToTarget(characterStand.transform, characterStand.heightPivot, node.positionOffset, node.rotationOffset, node.fovOffset);
            TrialDialogueManager.instance.effectController.Reset();
        }

        ((CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator)).ChangeFace(node.character.faceSprite);
        
        foreach (CameraEffect cameraEffect in node.cameraEffects)
        {
            TrialDialogueManager.instance.effectController.StartEffect(cameraEffect);
        }
        characterStand.SetSprite(node.character.emotions[node.expressionIndex]);

        yield return DialogueSystem.instance.Say(node);

    }

    public override IEnumerator Play()
    {
        CharacterStand characterStand = TrialManager.instance.characterStands.Find(stand => stand.character == character);
        if (!usePrevCamera)
        {
            TrialDialogueManager.instance.cameraController.TeleportToTarget(characterStand.transform, characterStand.heightPivot, positionOffset, rotationOffset, fovOffset);
            TrialDialogueManager.instance.effectController.Reset();
        }

        ((CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator)).ChangeFace(character.faceSprite);
        
        foreach (CameraEffect cameraEffect in cameraEffects)
        {
            TrialDialogueManager.instance.effectController.StartEffect(cameraEffect);
        }
        characterStand.SetSprite(character.emotions[expressionIndex]);

        yield return DialogueSystem.instance.Say(this);
    }
    
}