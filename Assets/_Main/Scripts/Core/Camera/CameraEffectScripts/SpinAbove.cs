using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Spin Above")]
public class SpinAbove : CameraEffect
{
    public override void Apply(CameraEffectController effectController)
    {
        effectController.cameraTransform.localPosition = new Vector3(0, 10, -8);
        effectController.cameraTransform.localRotation = Quaternion.Euler(33, 0, 0);
        effectController.camera.fieldOfView = 35f;

        CameraController.instance.pivot
            .DORotate(CameraController.instance.pivot.eulerAngles + Vector3.up * -360f, 40, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear)
            .SetTarget(effectController.cameraTransform);
    }
}
