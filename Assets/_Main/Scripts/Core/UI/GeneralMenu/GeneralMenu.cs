using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;

public class GeneralMenu : MonoBehaviour
{
    public List<MenuButton> menuItems;
    public int currentItemIndex;
    public int columns = 3;
    public AudioClip menuMove;
    public AudioClip menuSelect;
    public AudioClip menuFail;
    public LogoAnimation logoAnimator;
    public GameObject content;
    public GameObject logo;
    private Tween showContentTween;
    public RectTransform mainPart;
    public CanvasGroup mainPartCanvasGroup;
    private float originalPosY;
    private bool isInputActive;
    
    void Start()
    {
        originalPosY = mainPart.anchoredPosition.y;
    }

    void Update()
    {
        if (isInputActive)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (currentItemIndex > 0)
                    currentItemIndex--;

                UpdateCurrentItem();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (currentItemIndex < menuItems.Count - 1)
                    currentItemIndex++;
                UpdateCurrentItem();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (currentItemIndex < menuItems.Count - columns)
                    currentItemIndex += columns;
                UpdateCurrentItem();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (currentItemIndex - columns >= 0)
                    currentItemIndex -= columns;
                UpdateCurrentItem();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (menuItems[currentItemIndex].disabled)
                {
                    SoundManager.instance.PlaySoundEffect(menuFail);
                }
                else
                {
                    SoundManager.instance.PlaySoundEffect(menuSelect);
                    menuItems[currentItemIndex].Click();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerInputManager.instance.TogglePause();
        }
    }

    private void UpdateCurrentItem()
    {
        foreach (MenuButton menuButton in menuItems)
        {
            menuButton.StopHover();
        }

        SoundManager.instance.PlaySoundEffect(menuMove);

        if (!menuItems[currentItemIndex].disabled)
            menuItems[currentItemIndex].StartHover();
    }


    public void CloseMenu()
    {
        gameObject.SetActive(false);
        showContentTween?.Kill();
        logoAnimator.KillAll();
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        content.SetActive(false);
        isInputActive = false;
        logo.SetActive(true);
        logoAnimator.PlayAnimation();
        menuItems[currentItemIndex].StopHover();
        mainPartCanvasGroup.alpha = 0.4f;
        mainPart.DOAnchorPosY(originalPosY - 200, 0f).SetUpdate(true);

        showContentTween = DOVirtual.DelayedCall(Random.Range(1.5f, 4f), () =>
        {
            content.SetActive(true);
            logo.SetActive(false);
            currentItemIndex = 0;
            mainPartCanvasGroup.DOFade(1f, 0.8f).SetUpdate(true);
            mainPart.DOAnchorPosY(originalPosY, 0.8f).SetUpdate(true).OnComplete(() =>
            {
                if (!menuItems[currentItemIndex].disabled)
                    menuItems[currentItemIndex].StartHover();
                isInputActive = true;
            });
        });
    }
}