using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isAlreadyLooking = false;
    public virtual void Interact()
    {
        StartCoroutine(DoInteraction());
    }

    IEnumerator DoInteraction()
    {
        StartCoroutine(MoveAndRotateCameraTo());
        yield return StartCoroutine(PlayerInputManager.instance.shooter.ShootQuestionMark(this.transform.position));
        FinishInteraction();
        SoundManager.instance.PlaySoundEffect("click");
    }

    private IEnumerator MoveAndRotateCameraTo()
    {
        float duration = 0.5f;
        Quaternion targetRotation =
            Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);

        yield return CameraManager.instance.StartCameraCoroutine(CameraManager.instance.RotateCameraTo(targetRotation, duration));
    }

    public virtual void OnLook()
    {
        ReticleManager.instance.ShowOrHideMagnifyingGlass(true);
    }
    
    public virtual void OnStopLooking()
    {
        isAlreadyLooking = false;
        ReticleManager.instance.ShowOrHideMagnifyingGlass(false);
    }

    public abstract void FinishInteraction();
}