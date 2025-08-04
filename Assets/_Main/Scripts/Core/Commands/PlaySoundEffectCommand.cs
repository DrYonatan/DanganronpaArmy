using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlaySoundEffectCommand : Command
{
    public AudioClip clip;
    public float volume;
    public override void Execute()
    {
        SoundManager.instance.PlaySoundEffect(clip.name);
    }

    public void DrawGUI()
    {
        clip = (AudioClip)EditorGUILayout.ObjectField("Clip", clip, typeof(AudioClip), false);
        volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
    }

}
