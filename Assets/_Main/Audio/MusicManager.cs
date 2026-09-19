using DG.Tweening;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public static MusicManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void PlaySong(AudioClip song)
    {
        if (song == null)
            return;

        audioSource.DOKill();
        audioSource.volume = 1f;
        audioSource.Stop();
        audioSource.clip = song;
        audioSource.Play();

        if (VNUIAnimator.instance != null)
        {
            VNUIAnimator.instance.musicBoxContainer.StartBars();
            VNUIAnimator.instance.SetMusic(song.name);
        }
    }

    public void StopSong()
    {
        audioSource.DOFade(0f, 1f)
            .OnComplete(() =>
            {
                audioSource.Stop();
                audioSource.volume = 1f; // Reset for next time it plays
                audioSource.clip = null;
            });
        
        
        if (VNUIAnimator.instance != null)
        {
            VNUIAnimator.instance.musicBoxContainer.StopBars();
            VNUIAnimator.instance.musicName.text = "";
        }
    }

    public void PauseSong()
    {
        audioSource.Pause();
    }

    public void ResumeSong()
    {
        audioSource.UnPause();
    }
    
    internal void LowerVolume()
    {
        audioSource.volume /= 4;
    }
    internal void RaiseVolume()
    {
        audioSource.volume *= 4;
    }
}
