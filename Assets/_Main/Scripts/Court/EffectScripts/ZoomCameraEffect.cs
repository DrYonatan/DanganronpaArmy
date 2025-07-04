using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviour Editor/Camera Effect/Zoom")]
public class ZoomCameraEffect : CameraEffect
{
    [SerializeField] float zoom;
    [SerializeField] float speed;
    public override IEnumerator Apply(CameraEffectController effectController)
    {
        float elapsedTime = 0f;
        while(elapsedTime < timeLimit)
        {
            effectController.cameraTransform.position = 
            Vector3.Lerp(effectController.cameraTransform.position,
            effectController.cameraTransform.position + effectController.cameraTransform.forward * zoom,
            speed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
