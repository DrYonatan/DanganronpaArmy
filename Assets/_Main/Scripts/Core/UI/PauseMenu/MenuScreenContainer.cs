using DIALOGUE;
using UnityEngine;

public class MenuScreenContainer : MonoBehaviour
{
    public GameObject currentOpenMenu;
    public GeneralMenu generalMenu;
    public bool isSubmenuOpen;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !generalMenu.gameObject.activeSelf && PlayerInputManager.instance.isPaused)
        {
            CloseMenu();
        }
    }

    public void OpenGeneralMenu()
    {
        generalMenu.OpenMenu();
    }

    public void CloseGeneralMenu()
    {
        generalMenu.CloseMenu();
    }

    public void CloseMenu()
    {
        currentOpenMenu?.SetActive(false);
        isSubmenuOpen = false;
        generalMenu.gameObject.SetActive(true);
    }

    public void OpenMenu(GameObject menu)
    {
        generalMenu.gameObject.SetActive(false);
        isSubmenuOpen = true;
        menu.SetActive(true);
        currentOpenMenu = menu;
    }
}
