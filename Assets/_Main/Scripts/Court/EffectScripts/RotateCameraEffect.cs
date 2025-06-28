using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Behaviour Editor/Camera Effect/Rotate"))]
public class RotateCameraEffect : CameraEffect
{
    [SerializeField] Vector3 rotationLimit;
    [SerializeField] float speed;

    public override void OnStart(CameraEffectController effectController)
    {
        
    }

    public override void Apply(CameraEffectController effectController)
    {
        effectController.rotation = Vector3.MoveTowards(effectController.rotation,
            rotationLimit,
            speed * Time.deltaTime
            );
    }
}
