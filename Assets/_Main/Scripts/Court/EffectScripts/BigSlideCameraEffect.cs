using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviour Editor/Camera Effect/Slide/Big Slide")]
public class BigSlideCameraEffect : SlideCameraEffect
{
    public override IEnumerator Apply(CameraEffectController effectController)
    {
        originalPosition = effectController.cameraTransform.position;
        TeleportToFromDirection(effectController);

        float elapsedTime = 0f;

        float originalSpeed = speed;
        speed = 0.75f;
        while(elapsedTime < timeLimit)
        {
            if(elapsedTime > 0.2f)
            speed = originalSpeed;
            effectController.cameraTransform.position = Vector3.MoveTowards(effectController.cameraTransform.position, originalPosition, Time.deltaTime * speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
