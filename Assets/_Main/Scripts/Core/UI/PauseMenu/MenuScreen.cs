using DG.Tweening;
using UnityEngine;

public abstract class MenuScreen : MonoBehaviour
{
    public virtual void Open()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack).SetUpdate(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    
}