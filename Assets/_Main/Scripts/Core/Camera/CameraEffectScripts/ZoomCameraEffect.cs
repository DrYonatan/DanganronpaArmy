using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Zoom")]
public class ZoomCameraEffect : CameraEffect
{
    [SerializeField] float zoom;

    public override IEnumerator Apply(CameraEffectController effectController)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = effectController.cameraTransform.position;
        Vector3 targetPosition =
            effectController.cameraTransform.position + effectController.cameraTransform.forward * zoom;
        while (elapsedTime < timeLimit)
        {
            effectController.cameraTransform.position =
                Vector3.Lerp(startPosition,
                    targetPosition,
                    elapsedTime / timeLimit);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}