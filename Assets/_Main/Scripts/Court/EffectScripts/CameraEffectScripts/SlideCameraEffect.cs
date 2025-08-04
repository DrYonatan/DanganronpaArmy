using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Slide/Small Slide")]
public class SlideCameraEffect : CameraEffect
{
    public Vector3 direction; 
    [SerializeField] protected float speed = 1f;

    public override IEnumerator Apply(CameraEffectController effectController)
    {
        float elapsedTime = 0f;
        Vector3 targetPos = effectController.cameraTransform.localPosition + direction;
        while (elapsedTime < timeLimit)
        {
            effectController.cameraTransform.localPosition = Vector3.MoveTowards(effectController.cameraTransform.localPosition,
                targetPos, Time.deltaTime * speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}