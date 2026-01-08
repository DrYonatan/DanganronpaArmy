using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupAnimator : MonoBehaviour
{
    public Image image;
    public RectTransform imageContainer;
    public CanvasGroup imageCanvasGroup;
    private float originalX;
    private bool isInitialized;
    public AudioClip showPopupSound;
    public AudioClip hidePopupSound;
    public static PopupAnimator instance { get; private set; }
    
    public void Awake()
    {
        if (!isInitialized)
        {
            instance = this;
            originalX = imageContainer.anchoredPosition.x;
            isInitialized = true;
        }
    }

    public void MakeImageAppear(Sprite sprite)
    {
        imageContainer.gameObject.SetActive(true);
        image.sprite = sprite;
        imageContainer.anchoredPosition = new Vector2(originalX + 300f, 0);
        imageCanvasGroup.alpha = 0f;
        imageCanvasGroup.DOFade(1f, 0.3f).SetEase(Ease.Linear);
        imageContainer.DOAnchorPosX(originalX, 0.3f).SetEase(Ease.Linear);
        SoundManager.instance.PlaySoundEffect(showPopupSound);
    }

    public void MakeImageDisappear()
    {
        imageCanvasGroup.DOFade(0f, 0.3f).SetEase(Ease.Linear);
        imageContainer.DOAnchorPosX(originalX + 300f, 0.3f).OnComplete(() =>
        {
            imageContainer.gameObject.SetActive(false);
        });
        SoundManager.instance.PlaySoundEffect(hidePopupSound);
    }
}