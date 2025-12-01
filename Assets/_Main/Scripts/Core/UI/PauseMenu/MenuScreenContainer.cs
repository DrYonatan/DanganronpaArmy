using DIALOGUE;
using UnityEngine;

public class MenuScreenContainer : MonoBehaviour
{
    public MenuScreen currentOpenMenu;
    public GeneralMenu generalMenu;
    public bool isSubmenuOpen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isSubmenuOpen && !generalMenu.gameObject.activeSelf &&
            PlayerInputManager.instance.isPaused)
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
        currentOpenMenu?.Close();
        isSubmenuOpen = false;
        generalMenu.gameObject.SetActive(true);
    }

    public void OpenMenu(MenuScreen menu)
    {
        generalMenu.gameObject.SetActive(false);
        isSubmenuOpen = true;
        menu.Open();
        currentOpenMenu = menu;
    }
}