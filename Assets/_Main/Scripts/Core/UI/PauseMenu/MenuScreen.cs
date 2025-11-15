using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class MenuScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public virtual void Open()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack).SetUpdate(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        canvasGroup.alpha = 0;
    }
    
}