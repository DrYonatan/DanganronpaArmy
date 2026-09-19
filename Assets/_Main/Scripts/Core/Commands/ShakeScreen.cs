using System.Collections;
using CHARACTERS;
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
            duration: 0.8f,
            strength: new Vector3(2f, 2f, 0f),
            vibrato: 18,
            randomness: 45,
            snapping: false,
            fadeOut: true
        );

        GameObject character = VNCharacterManager.instance.GetSpeakerObject();

        character?.transform.DOShakePosition(
            duration: 0.6f,
            strength: new Vector3(26f, 23f, 0f),
            vibrato: 50,
            randomness: 10,
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