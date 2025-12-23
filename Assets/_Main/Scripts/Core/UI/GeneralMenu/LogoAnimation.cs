using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LogoAnimation : MonoBehaviour
{
    public Image blueTsadik;
    public RectTransform blueTsadikTransform;
    public Image logo;
    private float blueTsadikOriginalPosX;
    public List<RectTransform> dots;
    public Sequence dotSequence;

    void Start()
    {
        blueTsadikOriginalPosX = blueTsadikTransform.anchoredPosition.x;
    }

    public void PlayAnimation()
    {
        blueTsadik.fillAmount = 0f;
        logo.fillAmount = 0f;
        blueTsadikTransform.DOAnchorPosX(blueTsadikOriginalPosX, 0f).SetUpdate(true);

        foreach (RectTransform dotTransform in dots)
        {
            dotTransform.DOScale(0f, 0f).SetUpdate(true);
        }

        blueTsadik.DOFillAmount(1f, 1f).SetUpdate(true).OnComplete(() =>
        {
            blueTsadikTransform.DOAnchorPosX(blueTsadikOriginalPosX - 250f, 0.25f).SetEase(Ease.OutCirc)
                .SetUpdate(true).OnComplete(() =>
                {
                    blueTsadikTransform.DOAnchorPosX(blueTsadikOriginalPosX + 100f, 0.25f).SetEase(Ease.InBack)
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            logo.DOFillAmount(1f, 0.8f).SetUpdate(true);
                            dotSequence = DOTween.Sequence().SetUpdate(true);

                            for (int i = 0; i < dots.Count; i++)
                            {
                                RectTransform dot = dots[i];

                                dotSequence.Insert(i * 0.2f, dot.DOScale(1f, 0.5f));
                                dotSequence.Insert(i * 0.2f + 0.5f, dot.DOScale(0f, 0.8f));
                            }
                        });
                });
        });
    }

    public void KillAll()
    {
        blueTsadik.DOKill();
        blueTsadikTransform.DOKill();
        logo.DOKill();
        dotSequence.Kill();
    }
}