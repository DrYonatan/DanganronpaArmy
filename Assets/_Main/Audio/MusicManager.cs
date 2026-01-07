using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audio;
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

        if (VNUIAnimator.instance != null)
        {
            VNUIAnimator.instance.musicBoxContainer.StartBars();
            VNUIAnimator.instance.musicName.text = $"עכשיו מתנגן: {song?.name}";
        }
    }

    public void StopSong()
    {
        audio.Stop();
        if (VNUIAnimator.instance != null)
        {
            VNUIAnimator.instance.musicBoxContainer.StopBars();
            VNUIAnimator.instance.musicName.text = "";
        }
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
