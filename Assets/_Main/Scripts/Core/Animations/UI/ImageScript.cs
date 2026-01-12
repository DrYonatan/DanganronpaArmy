using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour
{
    public static ImageScript instance { get; private set; }

    public Image overlayImage;
    public CanvasGroup blackFade;

    CanvasGroup canvasGroup;
    
    private void Awake()
    {
        blackFade.alpha = 1f;
        instance = this;
    }

    public void Show(string imageName, float duration)
    {
        overlayImage.sprite = Resources.Load<Sprite>($"Images/{imageName}");
        ShowingOrHiding(canvasGroup, duration, 1f);
    }

    public void Hide(float duration)
    {
        ShowingOrHiding(canvasGroup, duration, 0f);
    }

    public void FadeToBlack(float duration)
    {
        ShowingOrHiding(blackFade, duration, 1f);
    }

    public void UnFadeToBlack(float duration)
    {
        ShowingOrHiding(blackFade, duration, 0f);
    }

    private void ShowingOrHiding(CanvasGroup canvasGroupToShowOrHide, float duration, float targetAlpha)
    {
        canvasGroupToShowOrHide.DOFade(targetAlpha, duration);
    }

}
