using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        blackFade.alpha = 1f;
        instance = this;
    }

    public void Show(Sprite image, bool flash, float duration = 0.2f)
    {
        overlayImage.sprite = image;
        OverlayTextBoxManager.instance.SetAsTextBox();
        OverlayTextBoxManager.instance.Show();
        ShowingOrHiding(canvasGroup, duration, 1f);
        
        if (flash)
        {
            Flash(duration);
        }
    }

    public void Hide(bool flash, float duration=0.2f)
    {
        OverlayTextBoxManager.instance.Hide();
        DialogueSystem.instance.UseInitialDialogueContainer();
        ShowingOrHiding(canvasGroup, duration, 0f);
             
        if (flash)
        {
            Flash(duration);
        }
    }

    private void Flash(float duration)
    {
        SoundManager.instance.PlaySoundEffect(flashSound);
        whiteFlash.DOFade(1f, duration).SetLoops(2, LoopType.Yoyo);
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