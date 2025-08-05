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
      // TODO make the music manager PlaySong get the actual song mp3 and not the name
      MusicManager.instance.PlaySong(music.name);
   }

   #if UNITY_EDITOR
   public override void DrawGUI()
   {
      music = (AudioClip)EditorGUILayout.ObjectField("Music", music, typeof(AudioClip), false);
      volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
   }
   #endif
}
