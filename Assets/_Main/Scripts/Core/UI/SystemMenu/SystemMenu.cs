using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemMenu : MenuScreen
{
    public List<TitleScreenActionButton> buttons;
    public VolumeSlidersMenu volumeSlidersMenu;

    public SaveSelectMenu saveMenu;
    public SaveSelectMenu loadMenu;

    public AudioClip menuMoveSound;

    private int buttonIndex;

    void Update()
    {
        if(!volumeSlidersMenu.isConcentrating)
           MenuControl();
        
        if (Input.GetKeyDown(KeyCode.Escape) && !volumeSlidersMenu.isConcentrating)
        {
            if (saveMenu.gameObject.activeSelf)
                CloseSaveMenu();
            else if (loadMenu.gameObject.activeSelf)
            {
                CloseLoadMenu();
            }
            else
            {
                PlayerInputManager.instance.pauseMenu.GoBackToGeneral();
            }
        }

        if (!volumeSlidersMenu.isActive && !saveMenu.gameObject.activeSelf && !loadMenu.gameObject.activeSelf)
        {
            ButtonsControl();
        }
    }

    private void MenuControl()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            buttons[buttonIndex].DisableHover();
            volumeSlidersMenu.isActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            buttons[buttonIndex].HoverButtonAnimation();
            volumeSlidersMenu.isActive = false;
        }
    }

    private void ButtonsControl()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            buttons[buttonIndex].DisableHover();
            buttonIndex = Math.Min(buttonIndex + 1, buttons.Count - 1);
            buttons[buttonIndex].HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(menuMoveSound);
        }

        if (Input.GetKeyDown(KeyCode.W))
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

    public void OpenSaveMenu()
    {
        saveMenu.gameObject.SetActive(true);
    }

    private void CloseSaveMenu()
    {
        saveMenu.gameObject.SetActive(false);
    }
    
    public void OpenLoadMenu()
    {
        loadMenu.gameObject.SetActive(true);
    }

    private void CloseLoadMenu()
    {
        loadMenu.gameObject.SetActive(false);
    }

    public void GoToTitleScreen()
    {
        StartCoroutine(GoToTitleScreenPipeline());
    }

    private IEnumerator GoToTitleScreenPipeline()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        DOTween.KillAll();
        SceneManager.LoadScene("TitleScreen");
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(GameStateManager.instance.persistentObject);
        Destroy(GameStateManager.instance.gameObject);
    }
}