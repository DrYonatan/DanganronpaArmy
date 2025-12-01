using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TitleScreenSubMenu : MonoBehaviour
{
    public List<TitleScreenMenuButton> buttons;
    public int currentItemIndex;
    public AudioClip selectionSound;

    public void Initialize()
    {
        if (buttons.Count > 0)
            buttons[currentItemIndex].HoverButtonAnimation();
        AppearAnimation();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            buttons[currentItemIndex].DisableHover();
            currentItemIndex = Math.Min(currentItemIndex + 1, buttons.Count - 1);
            buttons[currentItemIndex].HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(selectionSound);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            buttons[currentItemIndex].DisableHover();
            currentItemIndex = Math.Max(currentItemIndex - 1, 0);
            buttons[currentItemIndex].HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(selectionSound);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            buttons[currentItemIndex].Click();
        }
    }

    private void AppearAnimation()
    {
        int index = 0;

        foreach (TitleScreenMenuButton button in buttons)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            Vector3 targetRotation = Vector3.zero;
            
            if (index % 2 == 0)
            {
                buttonTransform.rotation = Quaternion.Euler(0f, -80f, 0f);
            }
            else
            {
                buttonTransform.rotation = Quaternion.Euler(0f, 80f, 0f);
            }

            buttonTransform.DORotate(targetRotation, 0.3f);

            index++;
        }
    }

    public void OutroAnimation()
    {
        int index = 0;

        foreach (TitleScreenMenuButton button in buttons)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            Vector3 targetRotation = Vector3.zero;
            
            if (index % 2 == 0)
            {
                targetRotation = new Vector3(0f, -80f, 0f);
            }
            else
            {
                targetRotation = new Vector3(0f, 80f, 0f);
            }

            buttonTransform.DORotate(targetRotation, 0.2f);

            index++;
        }
    }
}