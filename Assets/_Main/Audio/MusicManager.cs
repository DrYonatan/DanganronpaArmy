using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    public static MusicManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
        
    }

    public void PlaySong(AudioClip song)
    {
        audio.Stop();
        audio.clip = song;
        audio.Play();
    }

    public void StopSong()
    {
        audio.Stop();
    }

    internal void Pause()
    {
        audio.Pause();
    }
    internal void UnPause()
    {
        audio.UnPause();
    }
    internal void LowerVolume()
    {
        audio.volume /= 4;
    }
    internal void RaiseVolume()
    {
        audio.volume *= 4;
    }
}
