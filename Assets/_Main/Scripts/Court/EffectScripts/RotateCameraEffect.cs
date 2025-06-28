using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Behaviour Editor/Camera Effect/Rotate"))]
public class RotateCameraEffect : CameraEffect
{
    [SerializeField] Vector3 rotationLimit;
    [SerializeField] float speed;

    public override IEnumerator Apply(CameraEffectController effectController)
    {
        float elapsedTime = 0f;
        while(elapsedTime < timeLimit)
        {
            effectController.rotation = Vector3.MoveTowards(effectController.rotation,
            rotationLimit,
            speed * Time.deltaTime
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
