using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuScreen : MonoBehaviour
{
    public CanvasGroup logo;
    public GameObject content;
    public virtual void Open()
    {
        content.SetActive(false);
        gameObject.SetActive(true);
        logo.alpha = 1f;
        logo
            .DOFade(0.2f, 0.5f)
            .SetLoops(3, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() =>
            {
                logo.alpha = 0f;
                content.SetActive(true);
            });
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        logo.DOKill();
    }
    
}