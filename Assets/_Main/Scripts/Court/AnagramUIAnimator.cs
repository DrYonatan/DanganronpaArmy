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
    
    public RectTransform silhouette;

    void Start()
    {
        Color c = mist.color;
        c.a = 0f;
        mist.color = c;
        
        Color c2 = shade.color;
        c2.a = 0f;
        shade.color = c2;
        
        mist.DOFade(1f, 3f)
            .SetLoops(-1, LoopType.Yoyo) // -1 = infinite
            .SetEase(Ease.Linear);  
        shade.DOFade(1f, 1f)
            .SetLoops(-1, LoopType.Yoyo) // -1 = infinite
            .SetEase(Ease.Linear);  
        
        silhouette.DOShakeAnchorPos(5f, strength: new Vector2(5f, 5f), vibrato: 1, randomness: 90, snapping: false, fadeOut: false)
            .SetLoops(-1, LoopType.Restart);
    }
    void Update()
    {
        shade.transform.Rotate(0, 0 , -40f * Time.deltaTime);
    }
}
