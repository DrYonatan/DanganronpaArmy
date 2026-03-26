using System.Collections;
using DG.Tweening;
using DIALOGUE;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour
{
    public static ImageScript instance { get; private set; }

    public Image overlayImage;
    public CanvasGroup blackFade;
    public Image whiteFlash;
    public CanvasGroup canvasGroup;
    public AudioClip flashSound;

    public CanvasGroup animatedImageContainer;
    public VNAnimatedImage animatedImage;

    private void Awake()
    {
        blackFade.alpha = 1f;
        instance = this;
    }

    public void Show(Sprite image, bool flash, float duration = 0.4f)
    {
        overlayImage.sprite = image;
        OverlayTextBoxManager.instance.SetAsTextBox();
        OverlayTextBoxManager.instance.Show();
        ShowingOrHiding(canvasGroup, duration, 1f);

        if (flash)
        {
            Flash(duration, flashSound);
        }
    }

    public void Hide(bool flash, float duration = 0.4f)
    {
        OverlayTextBoxManager.instance.Hide();
        DialogueSystem.instance.UseInitialDialogueContainer();
        ShowingOrHiding(canvasGroup, duration, 0f);

        if (flash)
        {
            Flash(duration, flashSound);
        }
    }

    public void Flash(float duration, AudioClip sound)
    {
        SoundManager.instance.PlaySoundEffect(sound);
        whiteFlash.DOFade(1f, duration / 2).SetLoops(2, LoopType.Yoyo);
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

    public void CreateAnimatedImage(VNAnimatedImage image)
    {
        animatedImage = Instantiate(image, animatedImageContainer.transform);
        animatedImageContainer.DOFade(1f, 0.2f).SetEase(Ease.Linear);
    }

    public IEnumerator ForwardAnimatedImage()
    {
        if (animatedImage == null)
            yield break;

        yield return animatedImage.ForwardSegment();
    }

    public void RemoveAnimatedImage()
    {
        if (animatedImage == null)
            return;

        animatedImageContainer.DOFade(0f, 0.2f).SetEase(Ease.Linear)
            .OnComplete(() => Destroy(animatedImage.gameObject));
    }
}