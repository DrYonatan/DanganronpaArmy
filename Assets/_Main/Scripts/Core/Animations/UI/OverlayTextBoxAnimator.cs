using DG.Tweening;

public class OverlayTextBoxAnimator : BasicTextBoxAnimator
{
    public override void TextBoxAppear()
    {
        dialogueBoxCanvasGroup.DOFade(1f, 0.2f);
        namePlateCanvasGroup.DOFade(1f, 0.2f);
    }

    public override void TextBoxDisappear()
    {
        dialogueBoxCanvasGroup.DOFade(0f, 0.2f);
        namePlateCanvasGroup.DOFade(0f, 0.2f);
    }

    public override void ShowNamePlate()
    {
        namePlateCanvasGroup.DOFade(1f, 0.2f);
    }

    public override void HideNamePlate()
    {
        namePlateCanvasGroup.DOFade(0f, 0.2f);
    }
    
}
