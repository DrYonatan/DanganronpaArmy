
using DG.Tweening;
using UnityEngine;

public class DebateTextBoxAnimator : BasicTextBoxAnimator, IFaceable
{
    public DebateUIAnimator debateUIAnimator;
    public CharacterFaceController characterFace;
    public override void TextBoxAppear()
    {
        FadeIn();
        debateUIAnimator.ShowTextBox();
    }

    public void FadeIn()
    {
        dialogueBoxCanvasGroup.DOFade(1f, 0f);
        namePlateCanvasGroup.DOFade(1f, 0f);
    }
    

    public override void TextBoxDisappear()
    {
        debateUIAnimator.HideTextBox();
    }

    public override void ShowNamePlate()
    {
        
    }

    public override void HideNamePlate()
    {
        
    }

    public void FaceAppear()
    {
        
    }

    public bool IsVisible()
    {
        return characterFace.isVisible;
    }

    public void ChangeFace(Sprite sprite)
    {
        characterFace.SetFace(sprite);
    }
}
