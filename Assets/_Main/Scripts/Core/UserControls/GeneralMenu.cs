using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;
using UnityEngine.UI;

public class GeneralMenu : MonoBehaviour
{
    public List<MenuButton> menuItems;
    public int currentItemIndex;
    public Image menuTopEffect;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentItemIndex > 0)
                currentItemIndex--;
            UpdateCurrentItem();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentItemIndex < menuItems.Count - 1)
                currentItemIndex++;
            UpdateCurrentItem();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            menuItems[currentItemIndex].Click();
        } 
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
            PlayerInputManager.instance.TogglePause();
        }
    }

    public void UpdateCurrentItem()
    {
        foreach (MenuButton menuButton in menuItems)
        {
            menuButton.SetIsHovered(false);
        }

        menuItems[currentItemIndex].SetIsHovered(true);
    }


    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        currentItemIndex = 0;
        UpdateCurrentItem();
    }
}