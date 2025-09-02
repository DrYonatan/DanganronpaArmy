using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnagramUIAnimator : MonoBehaviour
{
    public RawImage mist;

    public RawImage shade;

    public RawImage stars;
    public RawImage stars2;
    
    public float growFactor = 1.2f;
    public float growDuration = 0.3f;
    
    public RectTransform silhouette;

    void Start()
    {
        Color c = mist.color;
        c.a = 0f;
        mist.color = c;
        
        mist.DOFade(1f, 3f)
            .SetLoops(-1, LoopType.Yoyo) // -1 = infinite
            .SetEase(Ease.Linear);  
        
        silhouette.DOShakeAnchorPos(5f, strength: new Vector2(5f, 5f), vibrato: 1, randomness: 90, snapping: false, fadeOut: false)
            .SetLoops(-1, LoopType.Restart);

        StartCoroutine(StarsGrow());

// Sequence for stars2 (same, but delayed start)



    }

    IEnumerator StarsGrow()
    {
        Sequence seq1 = DOTween.Sequence();
        seq1.Append(stars.transform.DOScale(growFactor, growDuration).SetEase(Ease.OutQuad));
        seq1.Append(stars.DOFade(0f, growDuration));
        seq1.Append(stars.transform.DOScale(1f, 0f));
        seq1.Append(stars.DOFade(1f, 0f));
        seq1.SetLoops(-1);
        
        yield return new WaitForSeconds(growDuration);
        
        Sequence seq2 = DOTween.Sequence();
        seq2.Append(stars2.transform.DOScale(growFactor * 0.8f, growDuration).SetEase(Ease.OutQuad));
        seq2.Append(stars2.DOFade(0f, growDuration));
        seq2.Append(stars2.transform.DOScale(1f, 0f));
        seq2.Append(stars2.DOFade(1f, 0f));
        seq2.SetLoops(-1);
    }
    void Update()
    {
        shade.transform.Rotate(0, 0 , -40f * Time.deltaTime);
    }
}
