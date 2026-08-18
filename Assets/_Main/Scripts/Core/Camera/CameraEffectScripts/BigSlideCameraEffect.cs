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
    [SerializeField] private float spinAngle = 45f;
    [SerializeField, Range(0f, 1f)] private float fastDistanceFraction = 0.9f;
    [SerializeField, Range(0f, 1f)] private float fastTimeFraction = 0.2f;

    public void TeleportToFromDirection(CameraEffectController effectController)
    {
        switch (fromDirection)
        {
            case Direction.Up:
                effectController.cameraTransform.position += effectController.cameraTransform.up * amount;
                break;
            case Direction.Right:
                CameraController.instance.pivot.Rotate(Vector3.up, -spinAngle, Space.World);
                break;
            case Direction.Left:
                CameraController.instance.pivot.Rotate(Vector3.up, spinAngle, Space.World);
                break;
            case Direction.Down:
                effectController.cameraTransform.position -= effectController.cameraTransform.up * amount;
                break;
        }
    }

    public override void Apply(CameraEffectController effectController)
    {
        switch (fromDirection)
        {
            case Direction.Up:
            case Direction.Down:
                Vector3 originalPosition = effectController.cameraTransform.position;
                TeleportToFromDirection(effectController);
                Vector3 startPosition = effectController.cameraTransform.position;

                Sequence positionSequence = DOTween.Sequence();
                positionSequence.Append(effectController.cameraTransform
                    .DOMove(Vector3.Lerp(startPosition, originalPosition, fastDistanceFraction), timeLimit * fastTimeFraction)
                    .SetEase(Ease.Linear));
                positionSequence.Append(effectController.cameraTransform
                    .DOMove(originalPosition, timeLimit * (1f - fastTimeFraction))
                    .SetEase(Ease.Linear));
                positionSequence.SetTarget(effectController.cameraTransform);
                break;
            case Direction.Right:
            case Direction.Left:
                Transform pivot = CameraController.instance.pivot;
                Quaternion originalRotation = pivot.rotation;
                TeleportToFromDirection(effectController);
                Quaternion startRotation = pivot.rotation;

                Sequence rotationSequence = DOTween.Sequence();
                rotationSequence.Append(pivot
                    .DORotate(Quaternion.Slerp(startRotation, originalRotation, fastDistanceFraction).eulerAngles, timeLimit * fastTimeFraction)
                    .SetEase(Ease.Linear));
                rotationSequence.Append(pivot
                    .DORotate(originalRotation.eulerAngles, timeLimit * (1f - fastTimeFraction))
                    .SetEase(Ease.Linear));
                rotationSequence.SetTarget(effectController.cameraTransform);
                break;
        }
    }
}
