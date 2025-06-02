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

    public void PlaySoundEffect(string soundEffectName)
    {
        // Load the clip
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Sound Effects/{soundEffectName}");
        if (clip == null)
        {
            Debug.LogWarning($"Sound effect '{soundEffectName}' not found!");
            return;
        }

        // Create a temporary GameObject with an AudioSource
        GameObject tempAudio = new GameObject($"TempAudio_{soundEffectName}");
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        // Destroy the GameObject after the clip finishes playing
        Destroy(tempAudio, clip.length);
    }

    public void PlayTextBoxSound()
    {
        textBoxAudio.Stop();
        textBoxAudio.Play();
    }
}
