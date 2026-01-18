using UnityEngine;
using UnityEngine.UI;

public class VolumeSlidersMenu : MonoBehaviour
{
    public bool isActive;
    
    [Header("UI")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button applyButton;

    [Header("Navigation")]
    public float changeSpeed = 0.5f; 

    private int currentIndex = 0; 

    private Slider[] sliders;

    void Awake()
    {
        sliders = new Slider[] { musicSlider, sfxSlider };

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        applyButton.onClick.AddListener(ApplySettings);

        UpdateSelectionVisual();
    }

    void Update()
    {
        if (isActive)
        {
            HandleNavigation();
            HandleSliderChange();
            HandleApply();
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
            currentIndex = Mathf.Min(sliders.Length - 1, currentIndex + 1);
            UpdateSelectionVisual();
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

    private void HandleApply()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ApplySettings();
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
    }

    private void ApplySettings()
    {
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    private void SetMusicVolume(float value)
    {
        MusicManager.instance.audioSource.volume = value;
    }

    private void SetSFXVolume(float value)
    {
        
    }
}
