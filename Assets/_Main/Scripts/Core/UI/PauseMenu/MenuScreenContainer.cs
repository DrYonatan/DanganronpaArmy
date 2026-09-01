using System.Collections;
using DIALOGUE;
using Unity.VisualScripting;
using UnityEngine;

public class MenuScreenContainer : MonoBehaviour
{
    public MenuScreen currentOpenMenu;
    public GeneralMenu generalMenu;
    public bool isOpen;
    public bool isSubmenuOpen;

    private void CloseCurrentMenu()
    {
        currentOpenMenu?.Close();
        isSubmenuOpen = false;
    }

    public IEnumerator CloseSubMenuCooldown()
    {
        yield return new WaitForSecondsRealtime(2f);
        isSubmenuOpen = false;
    }

    public void ClosePauseScreen()
    {
        CloseCurrentMenu();
        PlayerInputManager.instance.TogglePauseAndMenu();
    }

    public void OpenGeneralMenu()
    {
        isOpen = true;
        generalMenu.OpenMenu();
    }

    public void CloseGeneralMenu()
    {
        isOpen = false;
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