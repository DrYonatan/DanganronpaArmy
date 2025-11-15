using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class MenuScreen : MonoBehaviour
{
    public bool isOpen;
    public CanvasGroup canvasGroup;
    public virtual void Open()
    {
        isOpen = true;
        canvasGroup.alpha = 1;
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack).SetUpdate(true);
    }

    public virtual void Close()
    {
        isOpen = false;
        canvasGroup.alpha = 0;
    }
    
}