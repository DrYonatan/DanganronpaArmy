using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextBoxAnimations : MonoBehaviour
{
    public RectTransform namePlate;
    public RectTransform dialoguePart;
    protected CanvasGroup dialogueBoxCanvasGroup;
    protected CanvasGroup namePlateCanvasGroup;
    public float namePlateMoveAmount = 20f; // pixels to move up

    public float duration;

    public RectTransform namePlateOriginalPos;

    void Awake()
    {
        dialogueBoxCanvasGroup = dialoguePart.GetComponent<CanvasGroup>();
        namePlateCanvasGroup = namePlate.GetComponent<CanvasGroup>();
    }

    public virtual void TextBoxAppear()
    {
        dialogueBoxCanvasGroup.alpha = 0f;
        namePlateCanvasGroup.alpha = 0f;
        dialogueBoxCanvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuad);
        namePlateCanvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuad);
        namePlate.anchoredPosition -= new Vector2(0, namePlateMoveAmount);
        namePlate.DOAnchorPos(namePlateOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
    }

    public virtual void TextBoxDisappear()
    {
        dialogueBoxCanvasGroup.alpha = 1f;
        namePlateCanvasGroup.alpha = 1f;
        dialogueBoxCanvasGroup.DOFade(0f, duration).SetEase(Ease.InOutQuad);
        namePlateCanvasGroup.DOFade(0f, duration).SetEase(Ease.InOutQuad);
        namePlate.anchoredPosition = namePlateOriginalPos.anchoredPosition;
        namePlate.DOAnchorPos(namePlateOriginalPos.anchoredPosition - new Vector2(0, namePlateMoveAmount), duration).SetEase(Ease.OutQuad);        
    }

}
