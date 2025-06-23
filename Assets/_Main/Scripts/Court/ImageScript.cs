using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour
{
    public static ImageScript instance { get; private set; }

    Image image;
    CanvasGroup canvasGroup; 

    private void Awake()
    {
        instance = this;
        image = gameObject.GetComponent<Image>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }
    public void Show(string imageName, float duration)
    {
        image.sprite = Resources.Load<Sprite>($"Images/{imageName}");
        StartCoroutine(ShowingOrHiding(duration, 1f));
    }
    public void Hide(float duration)
    {
        StartCoroutine(ShowingOrHiding(duration, 0f));
    }



    public IEnumerator ShowingOrHiding(float duration, float targetAlpha)
    {        
        while (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime / duration);
            yield return null;
        }

    }
}
