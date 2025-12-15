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
            GoBackToGeneral();
        }
    }

    public void CloseCurrentMenu()
    {
        currentOpenMenu?.Close();
        isSubmenuOpen = false;
    }

    public void ClosePauseScreen()
    {
        CloseCurrentMenu();
        PlayerInputManager.instance.TogglePause();
    }

    public void OpenGeneralMenu()
    {
        generalMenu.OpenMenu();
    }

    public void CloseGeneralMenu()
    {
        generalMenu.CloseMenu();
    }

    public void GoBackToGeneral()
    {
        CloseCurrentMenu();
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