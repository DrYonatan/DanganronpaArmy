using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Explosion : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image outline;
    public Image explosive;
    public float duration;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        transform.localScale = Vector3.one * 0.1f;
        Color color = outline.color;
        color.a = 0f;
        outline.color = color;
    }
    
    public void Explode()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.5f, duration));
        seq.Join(outline.DOFade(1f, 0.1f));
        seq.Join(outline.rectTransform.DOLocalRotate(new Vector3(0, 0, 360), duration + 0.1f, RotateMode.FastBeyond360));
        seq.Join(explosive.DOFade(0f, duration / 2).SetDelay(duration / 2));
        seq.Join(outline.DOFade(0f, duration / 2).SetDelay(duration / 2));
        seq.OnComplete(() => Destroy(gameObject));
    }
}
