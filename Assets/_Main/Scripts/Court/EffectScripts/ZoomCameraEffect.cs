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
            effectController.zoom = Mathf.MoveTowards( 
            effectController.zoom,
            zoom,
            speed * Time.deltaTime
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
