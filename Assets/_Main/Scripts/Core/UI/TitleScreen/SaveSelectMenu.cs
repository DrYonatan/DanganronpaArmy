using System;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;

public class SaveSelectMenu : TitleScreenSubMenu
{
    public SaveSlotButton buttonPrefab;
    public RectTransform savesContainer;

    public float buttonHeight = 100f;
    public int visibleCount = 7;

    void Awake()
    {
        for (int i = 1; i < SaveManager.instance.saveSlotAmount; i++)
        {
            GenerateSaveSlot(i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            buttons[currentItemIndex].DisableHover();
            currentItemIndex = (currentItemIndex + 1) % buttons.Count;
            buttons[currentItemIndex].HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(selectionSound);
            UpdateScroll();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            buttons[currentItemIndex].DisableHover();
            currentItemIndex = (currentItemIndex - 1 + buttons.Count) %
                               buttons.Count;

            buttons[currentItemIndex].HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(selectionSound);
            UpdateScroll();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            buttons[currentItemIndex].Click();
        }
    }

    private void GenerateSaveSlot(int index)
    {
        SaveSlotButton button = Instantiate(buttonPrefab, savesContainer);
        button.slot = index;
        buttons.Add(button);
    }

    private void UpdateScroll()
    {
        savesContainer.DOAnchorPosY(Mathf.Max((currentItemIndex - (visibleCount - 1)) * (buttonHeight + 50), 0), 0.2f);
    }
}