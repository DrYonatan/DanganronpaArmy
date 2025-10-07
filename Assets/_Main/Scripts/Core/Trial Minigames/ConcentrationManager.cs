using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentrationManager : MonoBehaviour
{
    public static ConcentrationManager instance { get; private set; }
    public bool isActive;
    public GameObject concentrationSpace;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        DeactivateConcentration();
        if (isActive)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ActivateConcentration();
            }
        }
        
    }

    void ActivateConcentration()
    {
        CameraController.instance.camera.cullingMask = ~0;
        concentrationSpace.SetActive(true);
        Time.timeScale = 0.25f;
        CameraController.instance.camera.cullingMask = LayerMask.GetMask("Court Characters", "UI");
    }

    void DeactivateConcentration()
    {
        CameraController.instance.camera.cullingMask = ~0;
        concentrationSpace.SetActive(false);
        Time.timeScale = 1f;
    }
    
}
