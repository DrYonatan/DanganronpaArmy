using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image container;
    public TextMeshProUGUI optionLabel;
    public Color selectedColor;
    public Color unselectedColor;
    public float selectDuration;
    public float originalX;

    void Start()
    {
        Color startColor = unselectedColor;
        startColor.a = 0f;
        container.color = startColor;
        container.DOFade(1f, 0.2f);
        rectTransform = GetComponent<RectTransform>();
        optionLabel.color = Color.white;
    }

    public void OnSelect()
    {
        container.DOColor(selectedColor, selectDuration);
        rectTransform.DOAnchorPosX(originalX - 150f, selectDuration);
        optionLabel.DOColor(unselectedColor, selectDuration);
    }

    public void OnDeselect()
    {
        container.DOColor(unselectedColor, selectDuration);
        rectTransform.DOAnchorPosX(originalX, selectDuration);
        optionLabel.DOColor(Color.white, selectDuration);
    }
}
