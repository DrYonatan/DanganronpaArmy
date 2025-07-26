using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DebateUIAnimator : MonoBehaviour
{
    public RectTransform namePart;
    public RectTransform facePart;
    public RectTransform timePart;

    public RectTransform namePartOriginalPos;
    public RectTransform facePartOriginalPos;
    public RectTransform timePartOriginalPos;

    public float moveAmountY = 40f;
    public float moveAmountX = 150f;

    public float duration = 0.5f;

    public void DebateUIAppear()
    {
        namePart.anchoredPosition = namePartOriginalPos.anchoredPosition + new Vector2(0, -moveAmountY);
        facePart.anchoredPosition = facePartOriginalPos.anchoredPosition + new Vector2(moveAmountX, 0);
        timePart.anchoredPosition = timePartOriginalPos.anchoredPosition + new Vector2(0, moveAmountY);

        namePart.DOAnchorPos(namePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
        facePart.DOAnchorPos(facePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
        timePart.DOAnchorPos(timePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
    }
    public void DebateUIDisappear()
    {
        namePart.anchoredPosition = namePartOriginalPos.anchoredPosition;
        facePart.anchoredPosition = facePartOriginalPos.anchoredPosition;
        timePart.anchoredPosition = timePartOriginalPos.anchoredPosition;

        namePart.DOAnchorPos(namePartOriginalPos.anchoredPosition + new Vector2(0, -moveAmountY), duration).SetEase(Ease.OutQuad);
        facePart.DOAnchorPos(facePartOriginalPos.anchoredPosition + new Vector2(moveAmountX, 0), duration).SetEase(Ease.OutQuad);
        timePart.DOAnchorPos(timePartOriginalPos.anchoredPosition + new Vector2(0, moveAmountY), duration).SetEase(Ease.OutQuad);
    }
}
