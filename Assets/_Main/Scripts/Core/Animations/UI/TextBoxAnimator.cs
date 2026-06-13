using UnityEngine;
using DG.Tweening;
using DIALOGUE;

public class TextBoxAnimator : BasicTextBoxAnimator
{
    public float namePlateMoveAmount = 20f; // pixels to move up

    public float duration;

    public RectTransform namePlateOriginalPos;

    void Awake()
    {
        dialogueBoxCanvasGroup = dialoguePart.GetComponent<CanvasGroup>();
        namePlateCanvasGroup = namePlate.GetComponent<CanvasGroup>();
    }

    public override void TextBoxAppear()
    {
        if (textBoxVisible)
            return;
        
        textBoxVisible = true;
        dialogueBoxCanvasGroup.DOKill();
        namePlateCanvasGroup.DOKill();
        namePlate.DOKill();
        
        dialogueBoxCanvasGroup.alpha = 0f;
        namePlateCanvasGroup.alpha = 0f;
        
        dialogueBoxCanvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuad);
        ShowNamePlate();
    }

    public override void TextBoxDisappear()
    {
        if (!textBoxVisible)
            return;
        textBoxVisible = false;
        DialogueSystem.instance.ClearTextBox();
        dialogueBoxCanvasGroup.alpha = 1f;
        dialogueBoxCanvasGroup.DOFade(0f, duration).SetEase(Ease.InOutQuad);
        if(namePlateVisible)
           HideNamePlate();
        namePlateVisible = true; // Reset the name plate visibility to the default true so it won't pop in during the debate's finish nodes
    }

    public override void ShowNamePlate()
    {
        namePlateVisible = true;
        namePlateCanvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuad);
        namePlate.anchoredPosition -= new Vector2(0, namePlateMoveAmount);
        namePlate.DOAnchorPos(namePlateOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
    }
    public override void HideNamePlate()
    {
        namePlateVisible = false;
        namePlateCanvasGroup.alpha = 1f;
        namePlateCanvasGroup.DOFade(0f, duration).SetEase(Ease.InOutQuad);
        namePlate.anchoredPosition = namePlateOriginalPos.anchoredPosition;
        namePlate.DOAnchorPos(namePlateOriginalPos.anchoredPosition - new Vector2(0, namePlateMoveAmount), duration).SetEase(Ease.OutQuad); 
    }

}
