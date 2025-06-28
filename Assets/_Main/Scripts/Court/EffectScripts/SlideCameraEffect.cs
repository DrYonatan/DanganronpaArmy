using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    Up,
    Right,
    Left,
    Down,
}

[CreateAssetMenu(menuName ="Behaviour Editor/Camera Effect/Slide/Small Slide")]
public class SlideCameraEffect : CameraEffect
{
    public Direction fromDirection;
    [SerializeField] protected float amount = 1f;
    [SerializeField] protected float speed = 1f;
    protected Vector3 originalPosition;

    public void TeleportToFromDirection(CameraEffectController effectController)
    {
        switch (fromDirection)
        {
            case Direction.Up:
            effectController.position += effectController.transform.up * amount;
            break;
            case Direction.Right:
            effectController.position += effectController.transform.right * amount;
            break;
            case Direction.Left:
            effectController.position -= effectController.transform.right * amount;
            break;
            case Direction.Down:
            effectController.position -= effectController.transform.up * amount;
            break;
        }
    }

    public override IEnumerator Apply(CameraEffectController effectController)
    {
        originalPosition = effectController.position;
        TeleportToFromDirection(effectController);

        float elapsedTime = 0f;
        while(elapsedTime < timeLimit)
        {
            effectController.position = Vector3.MoveTowards(effectController.position, originalPosition, Time.deltaTime * speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
