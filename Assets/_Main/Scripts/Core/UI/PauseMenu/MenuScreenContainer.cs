using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenContainer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    void CloseMenu()
    {
        gameObject.SetActive(false);
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }
}
