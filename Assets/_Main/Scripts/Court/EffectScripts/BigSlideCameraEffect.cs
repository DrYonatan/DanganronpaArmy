using System.Collections;
using UnityEngine;

public enum Direction {
    Up,
    Right,
    Left,
    Down,
}

[CreateAssetMenu(menuName ="Behaviour Editor/Camera Effect/Slide/Big Slide")]
public class BigSlideCameraEffect : CameraEffect
{
    public Direction fromDirection;
    [SerializeField] private float amount = 1f;
    [SerializeField] private float speed = 1f;
    private Vector3 originalPosition;
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
    public override IEnumerator Apply(CameraEffectController effectController)
    {
        originalPosition = effectController.cameraTransform.position;
        TeleportToFromDirection(effectController);

        float elapsedTime = 0f;

        float originalSpeed = speed;
        speed = 3f;
        while(elapsedTime < timeLimit)
        {
            if(elapsedTime > 0.3f)
            speed = originalSpeed;
            effectController.cameraTransform.position = Vector3.MoveTowards(effectController.cameraTransform.position, originalPosition, Time.deltaTime * speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
