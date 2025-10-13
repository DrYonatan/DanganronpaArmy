using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayMusicCommand : Command
{
   public AudioClip music;
   public float volume = 1f;
   
   public override IEnumerator Execute()
   {
      MusicManager.instance.PlaySong(music);
      yield return  new WaitForSeconds(music.length);
   }

   #if UNITY_EDITOR
   public override void DrawGUI()
   {
      music = (AudioClip)EditorGUILayout.ObjectField("Music", music, typeof(AudioClip), false);
      volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
   }
   #endif
}
