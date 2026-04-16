using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class ShakeScreen : Command
{
    public bool isDramatic;

    public override IEnumerator Execute()
    {
        Shake();
        yield return null;
    }

    private void Shake()
    {
        CameraManager.instance.cameraTransform.DOShakePosition(
            0.8f,
            new Vector3(2f, 2f, 0f), // XY only
            vibrato: 30,
            randomness: 90,
            snapping: false,
            fadeOut: true
        );

        ImageScript.instance.Flash(0.4f,
            Resources.Load<AudioClip>($"Audio/Sound Effects/{(isDramatic ? "shock" : "whip")}"));
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        isDramatic = GUILayout.Toggle(isDramatic, "Dramatic");
    }
#endif
}