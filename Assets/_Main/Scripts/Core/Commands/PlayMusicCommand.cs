using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayMusicCommand : Command
{
   public AudioClip music;
   public float volume = 1f;
   public bool toggle = true;
   
   public override IEnumerator Execute()
   {
      if (toggle)
      {
         MusicManager.instance.PlaySong(music);
         yield return null;
      }
      else
      {
         MusicManager.instance.StopSong();
      }
   }

   #if UNITY_EDITOR
   public override void DrawGUI()
   {
      base.DrawGUI();
      toggle = EditorGUILayout.Toggle("Play", toggle);
      if (toggle)
      {
         music = (AudioClip)EditorGUILayout.ObjectField("Music", music, typeof(AudioClip), false);
         volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
      }
   }
   #endif
}
