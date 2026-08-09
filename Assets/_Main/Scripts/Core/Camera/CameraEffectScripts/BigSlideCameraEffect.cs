using DG.Tweening;
using UnityEngine;

public enum Direction {
    Up,
    Right,
    Left,
    Down,
}

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Slide/Big Slide")]
public class BigSlideCameraEffect : CameraEffect
{
    public Direction fromDirection;
    [SerializeField] private float amount = 1f;

    public void TeleportToFromDirection(CameraEffectController effectController)
    {
        switch (fromDirection)
        {
            case Direction.Up:
                effectController.cameraTransform.position += effectController.cameraTransform.up * amount;
                break;
            case Direction.Right:
                effectController.cameraTransform.position += effectController.cameraTransform.right * amount;
                break;
            case Direction.Left:
                effectController.cameraTransform.position -= effectController.cameraTransform.right * amount;
                break;
            case Direction.Down:
                effectController.cameraTransform.position -= effectController.cameraTransform.up * amount;
                break;
        }
    }

    public override void Apply(CameraEffectController effectController)
    {
        Vector3 originalPosition = effectController.cameraTransform.position;
        TeleportToFromDirection(effectController);
        effectController.cameraTransform.DOMove(originalPosition, timeLimit).SetEase(Ease.OutCubic);
    }
}
