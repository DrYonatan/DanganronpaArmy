using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviour Editor/Camera Effect/Slide/Big Slide")]
public class BigSlideCameraEffect : SlideCameraEffect
{
    public override IEnumerator Apply(CameraEffectController effectController)
    {
        originalPosition = effectController.position;
        TeleportToFromDirection(effectController);

        float elapsedTime = 0f;

        float originalSpeed = speed;
        speed = 0.75f;
        while(elapsedTime < timeLimit)
        {
            if(elapsedTime > 0.1f)
            speed = originalSpeed;
            effectController.position = Vector3.MoveTowards(effectController.position, originalPosition, Time.deltaTime * speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
