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
        Quaternion startRotation = effectController.cameraTransform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(rotationLimit);

        while(elapsedTime < timeLimit)
        {
            effectController.cameraTransform.rotation = Quaternion.RotateTowards(
            effectController.cameraTransform.rotation,
            targetRotation,
            speed * Time.deltaTime
        );

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
