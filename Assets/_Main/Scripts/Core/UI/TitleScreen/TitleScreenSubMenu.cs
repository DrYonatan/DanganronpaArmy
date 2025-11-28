using System;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenSubMenu : MonoBehaviour
{
    public List<TitleScreenMenuButton> buttons;
    public int currentItemIndex;

    public void Initialize()
    {
        if(buttons.Count > 0)
           buttons[currentItemIndex].HoverButtonAnimation();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            buttons[currentItemIndex].DisableHover();
            currentItemIndex = Math.Min(currentItemIndex + 1, buttons.Count - 1);
            buttons[currentItemIndex].HoverButtonAnimation();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            buttons[currentItemIndex].DisableHover();
            currentItemIndex = Math.Max(currentItemIndex - 1, 0);
            buttons[currentItemIndex].HoverButtonAnimation();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            buttons[currentItemIndex].Click();
        }
    }
}
