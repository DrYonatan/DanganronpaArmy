using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Shake")]
public class ShakeCameraEffect : CameraEffect
{
    [SerializeField] Vector3 limits;
    [SerializeField] int intensity = 10;

    public override void Apply(CameraEffectController effectController)
    {
        effectController.cameraTransform.DOShakePosition(timeLimit, limits / 100f, intensity, 90)
            .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}