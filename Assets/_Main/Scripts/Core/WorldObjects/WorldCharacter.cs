using System.Collections;
using UnityEngine;

public class WorldCharacter : ConversationInteractable
{
    public RoomIntroEffect appearEffect;
    public RoomIntroEffect appearEffectSilhouette;
    public Character character;

    protected override void FinishInteraction()
    {
        WorldManager.instance.StartCoroutine(FinishInteractionPipeline());
    }

    private IEnumerator FinishInteractionPipeline()
    {
        yield return CharacterClickEffects.instance.Interact(transform);
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.RotateCameraTo(targetRotation, 0.5f));
        CameraManager.instance.initialRotation = transform.rotation;
        
        float duration = 0.5f;
        Vector3 targetPosition =
            Vector3.Lerp(CameraManager.instance.cameraTransform.position, transform.position + Vector3.up * 0.8f, 0.75f);
        CameraManager.instance.StartCameraCoroutine(CameraManager.instance.MoveCameraTo(targetPosition, duration));

        StartConversation();
        
        if (ProgressManager.instance.currentGameEvent != null)
        {
            ((WorldEvent)ProgressManager.instance.currentGameEvent).charactersData[name] = new ObjectData(id, isClicked, clickCount);
        }
    }

    public override void OnLook()
    {
        if (!isAlreadyLooking && CursorManager.instance.cursor.gameObject.activeInHierarchy)
        {
            isAlreadyLooking = true;
            CursorManager.instance.ShowOrHideConversationIcon(true);
            CursorManager.instance.ShowOrHideInteractableName(true, character.displayName);
        }
    }

    public override void OnStopLooking()
    {
        isAlreadyLooking = false;
        CursorManager.instance.ShowOrHideConversationIcon(false);
        CursorManager.instance.ShowOrHideInteractableName(false, "");
    }

    public void AppearAnimation()
    {
        StartCoroutine(appearEffect.PlayEffect());
        StartCoroutine(appearEffectSilhouette.PlayEffect());
    }
}