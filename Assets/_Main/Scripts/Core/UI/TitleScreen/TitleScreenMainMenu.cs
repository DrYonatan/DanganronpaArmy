using System.Collections.Generic;
using UnityEngine;

public class TitleScreenMainMenu : MonoBehaviour
{
    public static TitleScreenMainMenu instance { get; private set; }
    
    public List<TitleScreenSubMenu> subMenus;
    public TitleScreenSubMenu activeSubMenu;
    private Stack<TitleScreenSubMenu> subMenuStack = new ();

    void Awake()
    {
        instance = this;
        subMenuStack.Push(activeSubMenu);
        Cursor.lockState =  CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& subMenuStack.Count > 1)
        {
            ReturnToPrevMenu();
        }
    }

    public void SwitchMenus(TitleScreenSubMenu menu)
    {
        subMenuStack.Push(menu);
        activeSubMenu.gameObject.SetActive(false);
        activeSubMenu = menu;
        activeSubMenu.gameObject.SetActive(true);
        activeSubMenu.Initialize();
    }

    private void ReturnToPrevMenu()
    {
        subMenuStack.Pop().gameObject.SetActive(false);
        activeSubMenu = subMenuStack.Peek();
        activeSubMenu.gameObject.SetActive(true);
        activeSubMenu.Initialize();
    }
}