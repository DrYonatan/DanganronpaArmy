using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Effect/Spin Above")]
public class SpinAbove : CameraEffect
{
    public override IEnumerator Apply(CameraEffectController effectController)
    {
        effectController.cameraTransform.localPosition = new Vector3(0, 10, -8);
        effectController.cameraTransform.localRotation = Quaternion.Euler(33, 0, 0);
        effectController.camera.fieldOfView = 35f;

        while (true)
        {
            CameraController.instance.pivot.Rotate(0f, -0.03f, 0f);
            yield return null;
        }
    }
}
