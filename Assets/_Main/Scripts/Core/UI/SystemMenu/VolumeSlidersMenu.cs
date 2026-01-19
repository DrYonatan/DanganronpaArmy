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

    [Header("Navigation")]
    public float changeSpeed = 0.5f;

    public AudioMixer mixer;

    private int currentIndex = 0; 

    private Slider[] sliders;

    public bool isConcentrating;

    void Awake()
    {
        sliders = new Slider[] { musicSlider, sfxSlider };

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        applyButton.button.onClick.AddListener(ApplySettings);

        UpdateSelectionVisual();
    }

    void Update()
    {
        if (isActive)
        {
            HandleNavigation();
            if(isConcentrating)
               HandleSliderChange();
        }
    }

    private void HandleNavigation()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            UpdateSelectionVisual();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentIndex = Mathf.Min(sliders.Length, currentIndex + 1);
            UpdateSelectionVisual();
        }

        if (PlayerInputManager.instance.DefaultInput())
        {
            if (currentIndex == sliders.Length)
            {
                applyButton.Click();
            }
            
            else if (!isConcentrating)
            {
                isConcentrating = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isConcentrating = false;
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
    }

    private void UpdateSelectionVisual()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            var colors = sliders[i].colors;
            colors.normalColor = (i == currentIndex) ? Color.yellow : Color.white;
            sliders[i].colors = colors;
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
