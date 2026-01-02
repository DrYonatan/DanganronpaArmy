using System.Collections;
using System.Collections.Generic;
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
        canvasGroup = overlayImage.GetComponent<CanvasGroup>();
        blackFade.alpha = 1f;
        instance = this;
    }

    public void Show(string imageName, float duration)
    {
        overlayImage.sprite = Resources.Load<Sprite>($"Images/{imageName}");
        StartCoroutine(ShowingOrHiding(canvasGroup, duration, 1f));
    }

    public void Hide(float duration)
    {
        StartCoroutine(ShowingOrHiding(canvasGroup, duration, 0f));
    }

    public void FadeToBlack(float duration)
    {
        StartCoroutine(ShowingOrHiding(blackFade, duration, 1f));
    }

    public void UnFadeToBlack(float duration)
    {
        StartCoroutine(ShowingOrHiding(blackFade, duration, 0f));
    }

    public IEnumerator ShowingOrHiding(CanvasGroup canvasGroupToShowOrHide, float duration, float targetAlpha)
    {        
        float elapsedTime = 0f;
        float startAlpha = canvasGroupToShowOrHide.alpha;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroupToShowOrHide.alpha = Mathf.MoveTowards(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroupToShowOrHide.alpha = targetAlpha;

    }

}
