using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NoThatsWrongAnimator : MonoBehaviour
{
    public float appearDuration;
    public Image topBar;
    public Image bottomBar;

    public Image background;
    public Image face;
    public Image counterImage;
    public RectTransform counterRect;
    
    public AudioClip counterSound;
    public AudioClip voiceLine;

    public IEnumerator Show()
    {
        SoundManager.instance.PlaySoundEffect(voiceLine);
        SoundManager.instance.PlaySoundEffect(counterSound);
        
        RectTransform topBarRect = topBar.GetComponent<RectTransform>();
        RectTransform bottomBarRect = bottomBar.GetComponent<RectTransform>();
        RectTransform backgroundRect = background.GetComponent<RectTransform>();
        RectTransform faceRect = face.GetComponent<RectTransform>();
        
        Color transparent = new Color(255, 255, 255, 0);

        topBarRect.anchoredPosition = new Vector2(0, 55);
        topBarRect.localRotation = Quaternion.Euler(0, 0, -9);
        topBar.color = transparent;
        
        bottomBarRect.anchoredPosition = new Vector2(0, -104);
        bottomBarRect.localRotation = Quaternion.Euler(0, 0, 2);
        bottomBar.color = transparent;
        
        backgroundRect.localScale = new Vector3(1, 0.4f, 1);
        background.color = transparent;
        
        face.color = transparent;
        counterImage.color = transparent;
        
        topBarRect.DOAnchorPosY(244f, appearDuration);
        topBarRect.DORotate(new Vector3(0f, 0f, -28f), appearDuration);
        topBar.DOFade(1f, appearDuration);
        
        bottomBarRect.DOAnchorPosY(-214, appearDuration);
        bottomBar.DOFade(1f, appearDuration);
        
        backgroundRect.DOScaleY(1, appearDuration * 1.2f); // background takes slightly longer to appear
        background.DOFade(1f, appearDuration * 1.2f);
        
        yield return new WaitForSeconds(appearDuration);

        faceRect.anchoredPosition = new Vector2(309f, -59f);
        faceRect.localScale = new Vector3(1, 0.72f, 1);
        faceRect.localRotation = Quaternion.Euler(0, 0, 9f);
        face.color = Color.white;
        
        faceRect.DOScaleY(1, appearDuration);
        faceRect.DORotate(new Vector3(0f, 0f, 0f), appearDuration);
        faceRect.DOAnchorPos(new Vector2(180f, -11f), appearDuration);
        
        yield return new WaitForSeconds(appearDuration);
        
        counterRect.anchoredPosition = new Vector2(524f, -120f);
        counterImage.color = Color.white;
        
        backgroundRect.DOAnchorPosY(10f, 0.05f).SetLoops(-1, LoopType.Yoyo);
        faceRect.DOAnchorPos(new Vector2(135f, -3f), 2f).SetEase(Ease.Linear);
        counterRect.DOAnchorPos(new Vector2(-16f, 15f), 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.2f);

        counterRect.DOAnchorPos(new Vector2(-200f, 60f), 1.8f).SetEase(Ease.Linear);
        counterImage.GetComponent<RectTransform>().DOAnchorPosY(15f, 0.05f).SetLoops(-1, LoopType.Yoyo);

        yield return new WaitForSeconds(1.8f);

        counterImage.DOKill();
        counterRect.DOKill();
        counterRect.DOAnchorPos(new Vector2(-732, 195), appearDuration).SetEase(Ease.Linear);
        face.DOFade(0f, appearDuration);

        yield return new WaitForSeconds(appearDuration);
        
        topBarRect.DOAnchorPosY(55f, appearDuration);
        topBarRect.DORotate(new Vector3(0f, 0f, -9f), appearDuration);
        topBar.DOFade(0f, appearDuration);
        
        bottomBarRect.DOAnchorPosY(-104, appearDuration);
        bottomBar.DOFade(0f, appearDuration);

        backgroundRect.DOKill();
        background.DOFade(0f, appearDuration);
        backgroundRect.DOScaleY(0.4f, appearDuration);
        
        yield return new WaitForSeconds(appearDuration);
    }
}
