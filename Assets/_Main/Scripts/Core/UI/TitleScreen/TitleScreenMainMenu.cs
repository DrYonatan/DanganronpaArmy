using System.Collections;
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
        StartCoroutine(SwitchMenusAnimation(activeSubMenu, menu));
        activeSubMenu = menu;
    }

    private void ReturnToPrevMenu()
    {
        StartCoroutine(SwitchMenusAnimation(subMenuStack.Pop(), subMenuStack.Peek()));
        activeSubMenu = subMenuStack.Peek();
    }

    private IEnumerator SwitchMenusAnimation(TitleScreenSubMenu prev, TitleScreenSubMenu next)
    {
        prev.OutroAnimation();
        
        yield return new WaitForSeconds(0.2f);
        
        prev.gameObject.SetActive(false);
        next.gameObject.SetActive(true);
        
        next.Initialize();
    }
}