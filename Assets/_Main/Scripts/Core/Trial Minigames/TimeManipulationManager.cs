using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeManipulationManager : MonoBehaviour
{
    public static TimeManipulationManager instance { get; private set; }
    public bool isInputActive;
    private bool isAlreadyConcentrating;
    private bool isCooldown;
    public GameObject concentrationSpace;
    public AudioClip concentrationSound;
    public float maxConsentration = 5f;
    public float concentration;

    void Awake()
    {
        if (instance == null)
            instance = this;
        concentration = maxConsentration;
    }

    void Update()
    {
        if (isInputActive)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (concentration == 0)
                {
                    isCooldown = true;
                    DOVirtual.DelayedCall(1f, () => {
                        isCooldown = false;
                    });
                    DeactivateConcentration();
                }
                    
                else if (!isAlreadyConcentrating && !isCooldown)
                { 
                    ActivateConcentration();
                }
                
            }
            
            else
            {
                if(isAlreadyConcentrating)
                   DeactivateConcentration();
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Time.timeScale = 4f;
                }
                else
                {
                    Time.timeScale = 1f;
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
        DOTween.To(() => concentration, x => concentration = x, 0f, concentration * 0.2f).SetEase(Ease.Linear).SetUpdate(true);
        TrialManager.instance.barsAnimator.DrainConcentration(concentration * 0.2f);
    }

    void DeactivateConcentration()
    {
        isAlreadyConcentrating = false;
        SoundManager.instance.StopSoundEffect(concentrationSound.name);
        CameraController.instance.camera.cullingMask = ~0;
        concentrationSpace.SetActive(false);
        Time.timeScale = 1f;
        float fillDuration = maxConsentration -
                             concentration;
        DOTween.To(() => concentration, x => concentration = x, maxConsentration, fillDuration).SetEase(Ease.Linear);
        TrialManager.instance.barsAnimator.FillConcentration(fillDuration);
    }
    
}
