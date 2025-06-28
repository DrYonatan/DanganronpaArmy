using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction {
    Up,
    Right,
    Left,
    Down,
}

[CreateAssetMenu(menuName ="Behaviour Editor/Camera Effect/Slide")]
public class SlideCameraEffect : CameraEffect
{
    [SerializeField] Direction fromDirection;
    [SerializeField] float amount = 1f;
    [SerializeField] float speed = 1f;
    Vector3 originalPosition;
    float elapsedTime = 0f;
    public override void OnStart(CameraEffectController effectController)
    {
        
        originalPosition = effectController.position;
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

    public override void Apply(CameraEffectController effectController)
    {
        effectController.position = Vector3.MoveTowards(effectController.position, originalPosition, Time.deltaTime * speed);
        elapsedTime += Time.deltaTime;
    }
}
