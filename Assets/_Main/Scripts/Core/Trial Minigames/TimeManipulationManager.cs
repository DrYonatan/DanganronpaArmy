using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManipulationManager : MonoBehaviour
{
    public static TimeManipulationManager instance { get; private set; }
    public bool isInputActive;
    public GameObject concentrationSpace;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (isInputActive)
        {
            DeactivateConcentration();
            if (Input.GetKey(KeyCode.Space))
            {
                ActivateConcentration();
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 4f;
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
        CameraController.instance.camera.cullingMask = ~0;
        concentrationSpace.SetActive(true);
        Time.timeScale = 0.25f;
        CameraController.instance.camera.cullingMask = LayerMask.GetMask("Concentration Visible");
    }

    void DeactivateConcentration()
    {
        CameraController.instance.camera.cullingMask = ~0;
        concentrationSpace.SetActive(false);
        Time.timeScale = 1f;
    }
    
}
