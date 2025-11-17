using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;
using UnityEngine.UI;

public class GotItAnimator : MonoBehaviour
{
    public RectTransform container;
    public RectTransform backgroundRect;
    public Image face;
    public AudioClip voiceLine;
    public AudioClip soundEffect;

    public float appearDuration;
    public float growFactor = 1.5f;
    public float stayDuration = 2f;

    public IEnumerator Show()
    {
        SoundManager.instance.PlaySoundEffect(voiceLine);
        SoundManager.instance.PlaySoundEffect(soundEffect);
        DialogueSystem.instance.ClearTextBox();
        container.localScale = new Vector3(growFactor, 0f, growFactor);
        container.GetComponent<CanvasGroup>().alpha = 0f;
        backgroundRect.anchoredPosition = new Vector2(46.237f, 0);
        face.rectTransform.anchoredPosition = new Vector2(0, 20f);
        
        container.DOScaleY(growFactor, appearDuration);
        container.GetComponent<CanvasGroup>().DOFade(1f, appearDuration);
        face.DOFade(0f, 0f);
        
        yield return new WaitForSeconds(appearDuration);

        container.localScale = Vector3.one;
        backgroundRect.DOAnchorPosY(20f, 0.05f).SetLoops(-1, LoopType.Yoyo);
        face.DOFade(1f, appearDuration);
        face.rectTransform.DOAnchorPosY(40f, stayDuration);
        
        yield return new WaitForSeconds(stayDuration);

        face.DOFade(0f, appearDuration);
        container.DOScaleY(0f, appearDuration);
        container.GetComponent<CanvasGroup>().DOFade(0f, appearDuration);

        yield return new WaitForSeconds(appearDuration);

        face.DOKill();
        backgroundRect.DOKill();
    }
}
