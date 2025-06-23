using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour
{
    public static ImageScript instance { get; private set; }

    public GameObject overlayImage;
    public GameObject blackFade;

    Image image;
    CanvasGroup canvasGroup;

    CanvasGroup blackFadeCanvasGroup;

    private void Awake()
    {
        image = overlayImage.GetComponent<Image>();
        canvasGroup = overlayImage.GetComponent<CanvasGroup>();
        blackFadeCanvasGroup = blackFade.GetComponent<CanvasGroup>();
        instance = this;
    }

    public void Show(string imageName, float duration)
    {
        image.sprite = Resources.Load<Sprite>($"Images/{imageName}");
        StartCoroutine(ShowingOrHiding(canvasGroup, duration, 1f));
    }

    public void Hide(float duration)
    {
        StartCoroutine(ShowingOrHiding(canvasGroup, duration, 0f));
    }

    public void FadeToBlack(float duration)
    {
        StartCoroutine(ShowingOrHiding(blackFadeCanvasGroup, duration, 1f));
    }

    public void UnFadeToBlack(float duration)
    {
        StartCoroutine(ShowingOrHiding(blackFadeCanvasGroup, duration, 0f));
    }

    public IEnumerator ShowingOrHiding(CanvasGroup canvasGroup, float duration, float targetAlpha)
    {        
        while (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime / duration);
            yield return null;
        }

    }

}
