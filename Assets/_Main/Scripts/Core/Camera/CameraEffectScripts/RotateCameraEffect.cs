using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = ("Behaviour Editor/Camera Effect/Rotate"))]
public class RotateCameraEffect : CameraEffect
{
    [SerializeField] Vector3 rotationLimit;

    public override void Apply(CameraEffectController effectController)
    {
        Quaternion targetRotation = effectController.cameraTransform.rotation * Quaternion.Euler(rotationLimit);
        effectController.cameraTransform.DORotateQuaternion(targetRotation, timeLimit).SetEase(Ease.Linear);
    }
}
