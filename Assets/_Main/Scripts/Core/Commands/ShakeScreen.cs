using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ShakeScreen : Command
{
    public override IEnumerator Execute()
    {
        Shake();
        yield return null;
    }

    private void Shake()
    {
        CameraManager.instance.cameraTransform.DOShakePosition(
            0.3f,
            new Vector3(1f, 1f, 0f), // XY only
            vibrato: 30,
            randomness: 90,
            snapping: false,
            fadeOut: true
        );
    }
}
