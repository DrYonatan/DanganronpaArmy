using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Dramatic Zoom")]
public class DramaticZoomCameraEffect : CameraEffect
{
    [SerializeField] float zoom;
    [SerializeField, Range(0f, 1f)] private float fastDistanceFraction = 0.9f;
    [SerializeField, Range(0f, 1f)] private float fastTimeFraction = 0.2f;

    public override void Apply(CameraEffectController effectController)
    {
        Vector3 originalPosition = effectController.cameraTransform.position;
        Vector3 targetPosition = originalPosition + effectController.cameraTransform.forward * zoom;
        Vector3 fastTarget =
            Vector3.Lerp(originalPosition, targetPosition, fastDistanceFraction);

        Sequence positionSequence = DOTween.Sequence();
        positionSequence.Append(effectController.cameraTransform
            .DOMove(fastTarget, timeLimit * fastTimeFraction)
            .SetEase(Ease.Linear));
        positionSequence.Append(effectController.cameraTransform
            .DOMove(targetPosition, timeLimit * (1f - fastTimeFraction))
            .SetEase(Ease.Linear));
        positionSequence.SetTarget(effectController.cameraTransform);
    }
}
