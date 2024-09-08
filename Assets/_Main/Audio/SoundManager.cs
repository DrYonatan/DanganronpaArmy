using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public AudioSource textBoxAudio;
    private void Awake()
    {
        instance = this;
        SetTextBoxAudio();
    }

    private void SetTextBoxAudio()
    {
        textBoxAudio = GameObject.Find("VN controller/Root/Canvas - Overlay/4 - Dialogue").GetComponent<AudioSource>();
        textBoxAudio.clip = Resources.Load<AudioClip>($"Audio/Sound Effects/textadvance");
    
    }

    public void PlaySoundEffect(string soundEffectName)
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>($"Audio/Sound Effects/{soundEffectName}");
        audio.Stop();
        audio.Play();
    }
    public void PlayTextBoxSound()
    {
        textBoxAudio.Stop();
        textBoxAudio.Play();
    }
}
