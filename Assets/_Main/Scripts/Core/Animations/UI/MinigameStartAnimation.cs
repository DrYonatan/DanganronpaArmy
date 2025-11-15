using DG.Tweening;
using UnityEngine;

public class MinigameStartAnimation : MonoBehaviour
{
    private RectTransform rectTransform;
    public RectTransform bullet;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Animate(float delay)
    {
        rectTransform.anchoredPosition = new Vector2(-1800, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.Append(rectTransform.DOAnchorPosX(0, 0.5f));
        sequence.Append(rectTransform.DOAnchorPosX(300, 1.2f).SetEase(Ease.Linear));
        sequence.Join(bullet.DOAnchorPosX(600, 1f)).SetEase(Ease.Linear);
        sequence.Append(rectTransform.DOAnchorPosX(2200, 0.2f));

        sequence.OnComplete(() => Destroy(gameObject));
    }
}
