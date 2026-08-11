using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Zoom")]
public class ZoomCameraEffect : CameraEffect
{
    [SerializeField] float zoom;

    public override void Apply(CameraEffectController effectController)
    {
        Vector3 targetPosition =
            effectController.cameraTransform.position + effectController.cameraTransform.forward * zoom;
        effectController.cameraTransform.DOMove(targetPosition, timeLimit).SetEase(Ease.Linear);
    }
}
