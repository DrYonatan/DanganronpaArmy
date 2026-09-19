using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Slide/Small Slide")]
public class SlideCameraEffect : CameraEffect
{
    public Vector3 direction;

    public override void Apply(CameraEffectController effectController)
    {
        Vector3 targetPos = effectController.cameraTransform.localPosition + direction;
        effectController.cameraTransform.DOLocalMove(targetPos, timeLimit).SetEase(Ease.Linear);
    }
}
