using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegulationsMenu : MenuScreen
{
    public TextMeshProUGUI ruleNumber;
    public TextMeshProUGUI ruleDescription;
    public RectTransform ruleDescriptionContainer;

    public List<string> regulations;

    public int currentRegulationIndex;

    public Image leftArrow;
    public Image rightArrow;

    public AudioClip moveSelectionSound;
    public float arrowGlowDuration = 0.2f;

    public Color arrowColor;
    public Color arrowGlowColor;

    public override void Open()
    {
        base.Open();
        currentRegulationIndex = 0;
        UpdateUI();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentRegulationIndex =
                currentRegulationIndex = Math.Clamp(currentRegulationIndex + 1, 0, regulations.Count - 1);
            UpdateUI();
            GlowArrow(rightArrow);
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentRegulationIndex = Math.Clamp(currentRegulationIndex - 1, 0, regulations.Count - 1);
            UpdateUI();
            GlowArrow(leftArrow);
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerInputManager.instance.pauseMenu.GoBackToGeneral();
        }
    }

    void UpdateUI()
    {
        ruleDescriptionContainer.DOKill();
        ruleDescriptionContainer.localScale = new Vector3(0, 1, 1);
        ruleDescriptionContainer.DOScaleX(1f, 1f).SetUpdate(true);
        ruleNumber.text = currentRegulationIndex + 1 + "";
        ruleDescription.text = regulations[currentRegulationIndex];
    }

    void GlowArrow(Image arrow)
    {
        arrow.DOKill();
        arrow.rectTransform.DOKill();
        arrow.color = arrowColor;
        arrow.DOColor(arrowGlowColor, arrowGlowDuration).SetLoops(2, LoopType.Yoyo).SetUpdate(true);
        arrow.rectTransform.DOScale(1.2f,  arrowGlowDuration).SetUpdate(true);
    }
}