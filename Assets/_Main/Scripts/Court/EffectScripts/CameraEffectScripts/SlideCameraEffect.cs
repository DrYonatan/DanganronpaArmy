using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Slide/Small Slide")]
public class SlideCameraEffect : CameraEffect
{
    public Vector3 direction; 
    [SerializeField] protected float speed = 1f;

    public override IEnumerator Apply(CameraEffectController effectController)
    {
        Transform cameraTransform = effectController.cameraTransform;
        Vector3 targetPosition = cameraTransform.position + direction.x * cameraTransform.right +
                                 direction.y * cameraTransform.up + direction.z * cameraTransform.forward;
        float elapsedTime = 0f;
        while (elapsedTime < timeLimit)
        {
            effectController.cameraTransform.position = Vector3.MoveTowards(effectController.cameraTransform.position,
                targetPosition, Time.deltaTime * speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}