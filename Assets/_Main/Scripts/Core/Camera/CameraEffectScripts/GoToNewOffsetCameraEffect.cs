using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Go to new offset")]
public class GoToNewOffsetsCameraEffect : CameraEffect
{
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public float fovOffset;

    public override void Apply(CameraEffectController effectController)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(effectController.cameraTransform.DOLocalMove(positionOffset, timeLimit));
        sequence.Join(effectController.cameraTransform.DOLocalRotate(rotationOffset, timeLimit));
        sequence.Join(DOTween.To(() => effectController.camera.fieldOfView, x => effectController.camera.fieldOfView = x, fovOffset, timeLimit));
        sequence.SetTarget(effectController.cameraTransform);
    }
}
