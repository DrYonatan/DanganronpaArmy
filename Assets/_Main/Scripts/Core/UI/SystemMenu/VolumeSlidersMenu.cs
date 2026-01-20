using DG.Tweening;
using DIALOGUE;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlidersMenu : MonoBehaviour
{
    public bool isActive;
    
    [Header("UI")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public TitleScreenActionButton applyButton;

    public Image musicFrame;
    public Image sfxFrame;

    [Header("Navigation")]
    public float changeSpeed = 0.5f;

    public AudioMixer mixer;

    private int currentIndex = 0; 

    private Slider[] sliders;
    private Image[] frames;

    public bool isConcentrating;

    public AudioClip chooseSound;
    public AudioClip moveSound;

    void Awake()
    {
        sliders = new Slider[] { musicSlider, sfxSlider };
        frames = new Image[] { musicFrame, sfxFrame };

    musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        applyButton.button.onClick.AddListener(ApplySettings);

        UpdateSelectionVisual();
    }

    void Update()
    {
        if (isActive)
        {
            if(isConcentrating)
               HandleSliderChange();
            else
            {
                HandleNavigation();
            }
        }
    }

    private void HandleNavigation()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            UpdateSelectionVisual();
            SoundManager.instance.PlaySoundEffect(moveSound);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentIndex = Mathf.Min(sliders.Length, currentIndex + 1);
            UpdateSelectionVisual();
            SoundManager.instance.PlaySoundEffect(moveSound);
        }

        if (PlayerInputManager.instance.DefaultInput())
        {
            if (currentIndex == sliders.Length)
            {
                applyButton.Click();
            }
            
            else if (!isConcentrating)
            {
                SoundManager.instance.PlaySoundEffect(chooseSound);
                var colors = sliders[currentIndex].colors;
                colors.normalColor = Color.blue;
                sliders[currentIndex].colors = colors;
                isConcentrating = true;
            }
        }
    }

    private void HandleSliderChange()
    {
        float input = 0f;

        if (Input.GetKey(KeyCode.A)) input = -1f;
        if (Input.GetKey(KeyCode.D)) input = 1f;

        if (input != 0f)
        {
            Slider current = sliders[currentIndex];
            current.value = Mathf.Clamp01(
                current.value + input * changeSpeed * Time.unscaledDeltaTime
            );
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var colors = sliders[currentIndex].colors;
            colors.normalColor = Color.gray;
            sliders[currentIndex].colors = colors;
            isConcentrating = false;
        }
    }

    public void UpdateSelectionVisual()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            if (i == currentIndex && isActive)
            {
                frames[i].DOKill();
                frames[i].DOFade(1f, 0.2f).SetUpdate(true);
            }
            else
            {
                frames[i].DOKill();
                frames[i].DOFade(0f, 0.2f).SetUpdate(true);
            }
        }

        if (currentIndex == sliders.Length)
        {
            applyButton.HoverButtonAnimation();
        }
        else
        {
            applyButton.DisableHover();
        }
    }

    private void ApplySettings()
    {
        SetMusicVolume(musicSlider.value);
        SetSfxVolume(sfxSlider.value);

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    private void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
    }

    private void SetSfxVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
    }
}
