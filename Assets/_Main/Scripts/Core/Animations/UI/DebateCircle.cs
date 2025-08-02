using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DebateCircle : MonoBehaviour
{
    public float direction = 1f;
    public float speed = 20f;
    public float scaleMultiplier = 2f;
    void Update()
    {
        transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, 1f) * (direction * speed * Time.deltaTime));
    }

    public void GrowAndShrink(float duration)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * scaleMultiplier;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(targetScale, duration).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOScale(originalScale, duration).SetEase(Ease.InQuad));
    }
}
