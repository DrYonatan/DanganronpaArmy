using DG.Tweening;
using UnityEngine;

public class CourtTextBoxAnimator : TextBoxAnimations
{
    public CharacterFaceController characterFace;
    public RectTransform backgroundTextContainer;

    public void FaceAppear()
    {
        characterFace.DiscussionFaceContainerAppear(duration);
    }

    public override void TextBoxDisappear()
    {
        base.TextBoxDisappear();

        backgroundTextContainer.DOKill();
        backgroundTextContainer.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);
        characterFace.DiscussionFaceContainerDisappear(duration);
    }

    public void ChangeFace(Sprite sprite)
    {
        characterFace.SetFace(sprite);
    }

    public override void Initialize()
    {
        base.Initialize();
        characterFace.discussionFaceContainer.DOScaleY(0f, 0f);
    }

    public CharacterCutIn InstantiateCutIn(CharacterCutIn cutIn)
    {
        return Instantiate(cutIn, TrialManager.instance.globalUI);
    }

    public ScreenShatterManager InstantiateScreenShatter(ScreenShatterManager screenShatter)
    {
        return Instantiate(screenShatter);
    }

    public void AnimateBackgroundText()
    {
        backgroundTextContainer.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
        backgroundTextContainer.DOKill();
        backgroundTextContainer.anchoredPosition = new Vector2(-77, 495);
        backgroundTextContainer.DOAnchorPosX(-498, 3f).SetEase(Ease.Linear).SetLoops(-1);
    }
    
}