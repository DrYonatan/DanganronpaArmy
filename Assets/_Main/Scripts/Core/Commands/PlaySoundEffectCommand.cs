using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlaySoundEffectCommand : Command
{
    public AudioClip clip;
    public float volume = 1f;
    public override IEnumerator Execute()
    {
        SoundManager.instance.PlaySoundEffect(clip);
        yield return new WaitForSeconds(clip.length);
    }

    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        clip = (AudioClip)EditorGUILayout.ObjectField("Clip", clip, typeof(AudioClip), false);
        volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
    }
    #endif

}
