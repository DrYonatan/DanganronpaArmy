using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManipulationManager : MonoBehaviour
{
    public static TimeManipulationManager instance { get; private set; }
    public bool isInputActive;
    private bool isAlreadyConcentrating;
    public GameObject concentrationSpace;
    public AudioClip concentrationSound;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (isInputActive)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (!isAlreadyConcentrating)
                   ActivateConcentration();
            }
            
            else
            {
                if(isAlreadyConcentrating)
                   DeactivateConcentration();
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Time.timeScale = 4f;
                }
            }
            
        }
        
    }

    public void DeActivateInput()
    {
        isInputActive = false;
        DeactivateConcentration();
    }

    void ActivateConcentration()
    {
        isAlreadyConcentrating = true;
        SoundManager.instance.PlaySoundEffect(concentrationSound);
        CameraController.instance.camera.cullingMask = ~0;
        concentrationSpace.SetActive(true);
        Time.timeScale = 0.25f;
        CameraController.instance.camera.cullingMask = LayerMask.GetMask("Concentration Visible");
        TrialManager.instance.barsAnimator.DrainConcentration(TrialManager.instance.playerStats.concentration * 0.2f);
    }

    void DeactivateConcentration()
    {
        isAlreadyConcentrating = false;
        SoundManager.instance.StopSoundEffect(concentrationSound.name);
        CameraController.instance.camera.cullingMask = ~0;
        concentrationSpace.SetActive(false);
        Time.timeScale = 1f;
        TrialManager.instance.barsAnimator.FillConcentration((TrialManager.instance.playerStats.maxConsentration - TrialManager.instance.playerStats.concentration) * 0.5f);
    }
    
}
