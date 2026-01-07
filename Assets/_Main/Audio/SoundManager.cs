using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public AudioSource textBoxAudio;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip == null)
            return;
        // Create a temporary GameObject with an AudioSource
        GameObject tempAudio = new GameObject($"TempAudio_{clip.name}");
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        // Destroy the GameObject after the clip finishes playing
        Destroy(tempAudio, clip.length);
    }

    public void StopSoundEffect(string clipName)
    {
        Destroy(GameObject.Find($"TempAudio_{clipName}"));
    }

    public void PlayTextBoxSound()
    {
        textBoxAudio.Stop();
        textBoxAudio.Play();
    }
}
