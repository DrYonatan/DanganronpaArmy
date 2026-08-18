using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = ("Behaviour Editor/Camera Effect/Change Fov"))]
public class FovChangeEffect : CameraEffect
{
    public float targetFov;

    public override void Apply(CameraEffectController effectController)
    {
        DOTween.To(() => effectController.camera.fieldOfView, x => effectController.camera.fieldOfView = x, targetFov, timeLimit)
            .SetEase(Ease.Linear)
            .SetTarget(effectController.cameraTransform);
    }
}
