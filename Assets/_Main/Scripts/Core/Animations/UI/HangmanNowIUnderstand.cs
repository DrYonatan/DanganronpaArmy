using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HangmanNowIUnderstand : MonoBehaviour
{
    public float appearDuration;
    public RawImage topBar;
    public RawImage bottomBar;

    public RawImage background;
    public Image face;

    public RawImage glow;

    public AudioClip openSound;
    public AudioClip voiceLine;
    public IEnumerator Show()
    {
        RectTransform topBarRect = topBar.GetComponent<RectTransform>();
        RectTransform bottomBarRect = bottomBar.GetComponent<RectTransform>();
        RectTransform backgroundRect = background.GetComponent<RectTransform>();
        face.gameObject.SetActive(false);
        glow.DOFade(0f, 0f);
        
        topBarRect.anchoredPosition = new Vector2(topBarRect.anchoredPosition.x, 16);
        bottomBarRect.anchoredPosition = new Vector2(bottomBarRect.anchoredPosition.x, -82);
        topBar.DOFade(0f, 0f);
        bottomBar.DOFade(0f, 0f);
        background.DOFade(0f, 0f);
        backgroundRect.DOScaleY(0f, 0f);
        SoundManager.instance.PlaySoundEffect(openSound);
        SoundManager.instance.PlaySoundEffect(voiceLine);
        
        topBarRect.DOAnchorPosY(162, appearDuration).SetEase(Ease.Linear);
        bottomBarRect.DOAnchorPosY(-162f, appearDuration).SetEase(Ease.Linear);
        topBar.DOFade(1f, appearDuration);
        bottomBar.DOFade(1f, appearDuration);
        background.DOFade(1f, appearDuration);
        backgroundRect.DOScaleY(1f, appearDuration);

        yield return new WaitForSeconds(appearDuration);
        
        face.gameObject.SetActive(true);
        face.GetComponent<RectTransform>()
            .DOScale(1.1f, 0.1f)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo);
        
        Vector2 backgroundPos = backgroundRect.anchoredPosition;
        
        backgroundRect.DOAnchorPosY(backgroundPos.y + 20f, 0.05f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        
        yield return new WaitForSeconds(0.5f);

        glow.DOFade(1f, 0.2f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
        glow.GetComponent<RectTransform>().DOScaleY(1f, 0.4f);

        yield return new WaitForSeconds(0.2f);

        topBar.DOFade(0f, 0f);
        bottomBar.DOFade(0f, 0f);
        background.DOFade(0f, 0f);
        face.gameObject.SetActive(false);
        

        yield return new WaitForSeconds(0.2f);
    }
}
