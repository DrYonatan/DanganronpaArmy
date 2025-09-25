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

    public void OnClick()
    {
        DOTween.Kill(container);

        Sequence seq = DOTween.Sequence();

        int flashes = 2;
        float delay = 0.05f;

        for (int i = 0; i < flashes; i++)
        {
            seq.AppendCallback(() => container.color = unselectedColor);
            seq.AppendInterval(delay);
            seq.AppendCallback(() => container.color = selectedColor);
            seq.AppendInterval(delay);
        }

        seq.OnComplete(() => container.color = selectedColor);
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

    public void OnExit()
    {
        optionLabel.DOFade(0f, 0f);
        container.DOFade(0f, 0.2f).OnComplete(() => Destroy(gameObject));
    }
}
