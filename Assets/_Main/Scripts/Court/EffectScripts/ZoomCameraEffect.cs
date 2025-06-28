using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviour Editor/Camera Effect/Zoom")]
public class ZoomCameraEffect : CameraEffect
{
    [SerializeField] float zoom;
    [SerializeField] float speed;
    public override void Apply(CameraEffectController effectController)
    {
        effectController.zoom = Mathf.MoveTowards(
            effectController.zoom,
            zoom,
            speed * Time.deltaTime
            );
    }
    public override void OnStart(CameraEffectController effectController)
    {
        
    }
}
