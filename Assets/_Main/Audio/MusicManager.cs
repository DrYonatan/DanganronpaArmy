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
        
        audioSource.Stop();
        audioSource.clip = song;
        audioSource.Play();

        if (VNUIAnimator.instance != null)
        {
            VNUIAnimator.instance.musicBoxContainer.StartBars();
            VNUIAnimator.instance.musicName.text = $"עכשיו מתנגן: {song?.name}";
        }
    }

    public void StopSong()
    {
        audioSource.Stop();
        if (VNUIAnimator.instance != null)
        {
            VNUIAnimator.instance.musicBoxContainer.StopBars();
            VNUIAnimator.instance.musicName.text = "";
        }
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
