using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    public List<MenuButton> menuItems;
    public int currentItemIndex;
    public Image menuTopEffect;
    public MenuScreenContainer menuScreenContainer;
    // Start is called before the first frame update
    void Start()
    {
        currentItemIndex = 0;
        UpdateCurrentItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (!menuScreenContainer.gameObject.activeSelf)
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
}