using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPiece : MonoBehaviour
{
    public RawImage image;
    RectTransform rectTransform;
    public Vector2 direction;
    public float speed;
    public float angularVelocity;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public IEnumerator Move(float duration)
    {
        float elapsedTime = 0f;
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = rectTransform.anchoredPosition + direction.normalized * (speed * duration);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            rectTransform.Rotate(0f, 0f, angularVelocity * Time.deltaTime);
            yield return null;
        }
    }
    
}
