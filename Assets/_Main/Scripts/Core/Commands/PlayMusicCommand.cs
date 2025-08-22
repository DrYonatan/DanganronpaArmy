using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayMusicCommand : Command
{
   public AudioClip music;
   public float volume = 1f;
   
   public override void Execute()
   {
      MusicManager.instance.PlaySong(music);
   }

   #if UNITY_EDITOR
   public override void DrawGUI()
   {
      music = (AudioClip)EditorGUILayout.ObjectField("Music", music, typeof(AudioClip), false);
      volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
   }
   #endif
}
