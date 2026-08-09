using DG.Tweening;
using UnityEngine;

public class CameraEffectController : MonoBehaviour
{
    public Transform cameraTransform;
    public Camera camera;

    void Start()
    {
        camera = cameraTransform.GetComponent<Camera>();
    }

    public void StartEffect(CameraEffect effect)
    {
        if (effect != null)
        {
            effect.Apply(this);
        }
    }

    public void Reset()
    {
        if (cameraTransform != null) DOTween.Kill(cameraTransform);
        if (camera != null) DOTween.Kill(camera);
    }
}
