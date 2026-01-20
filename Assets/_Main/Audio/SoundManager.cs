using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public AudioSource textBoxAudio;
    public AudioMixer mixer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        ApplySettings();
    }

    private void ApplySettings()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        
        mixer.SetFloat("MusicVolume", Mathf.Log10(music) * 20f);
        mixer.SetFloat("SFXVolume", Mathf.Log10(sfx) * 20f);
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip == null)
            return;
        // Create a temporary GameObject with an AudioSource
        GameObject tempAudio = new GameObject($"TempAudio_{clip.name}");
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
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
