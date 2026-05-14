using System;
using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public class SavePopup : MonoBehaviour
{
    public SaveSelectMenu saveMenu;
    public List<TitleScreenActionButton> buttons;
    private int buttonIndex;
    public AudioClip menuMoveSound;
    public bool finished;
    public bool saveMenuOpen;
    public GameObject foregroundElement;
    
    public IEnumerator WaitForCompletion()
    {
        yield return new WaitUntil(() => finished);
    }

    public void OpenSaveSelect()
    {
        saveMenuOpen = true;
        saveMenu.gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        finished = true;
        saveMenuOpen = false;
        foregroundElement.SetActive(false);
    }

    void Update()
    {
        if (saveMenuOpen)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                ClosePopup();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            buttons[buttonIndex].DisableHover();
            buttonIndex = Math.Min(buttonIndex + 1, buttons.Count - 1);
            buttons[buttonIndex].HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(menuMoveSound);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            buttons[buttonIndex].DisableHover();
            buttonIndex = Math.Max(buttonIndex - 1, 0);
            buttons[buttonIndex].HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(menuMoveSound);
        }

        if (PlayerInputManager.instance.DefaultInput())
        {
            buttons[buttonIndex].Click();
        }
    }
    
}
