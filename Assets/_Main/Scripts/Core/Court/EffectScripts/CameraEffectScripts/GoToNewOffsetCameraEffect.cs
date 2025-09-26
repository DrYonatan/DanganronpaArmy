using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Go to new offset")]
public class GoToNewOffsetsCameraEffect : CameraEffect
{
    
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public float fovOffset;
    
    public override IEnumerator Apply(CameraEffectController effectController)
    {
        float elapsedTime = 0;
        Vector3 startPos = effectController.cameraTransform.localPosition;
        Quaternion startRotation = effectController.cameraTransform.localRotation;
        float startFov = effectController.camera.fieldOfView;
        while (elapsedTime < timeLimit)
        {
            elapsedTime += Time.deltaTime;
            effectController.cameraTransform.localPosition = Vector3.Lerp(startPos, positionOffset, elapsedTime / timeLimit);
            effectController.cameraTransform.localRotation = Quaternion.Slerp(startRotation,Quaternion.Euler(rotationOffset) ,elapsedTime / timeLimit);
            effectController.camera.fieldOfView = Mathf.Lerp(startFov, fovOffset, elapsedTime / timeLimit);
            yield return null;
        }
        
    }
}
