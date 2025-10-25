using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ComicPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public List<ComicSpriteAnimation> spriteAnimations = new List<ComicSpriteAnimation>();

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }
    public IEnumerator Play()
    {
        canvasGroup.DOFade(1f, 0.1f);
        int animationsRunning = spriteAnimations.Count;
        foreach (ComicSpriteAnimation spriteAnimation in spriteAnimations)
        {
            StartCoroutine(PlaySpriteAnimation(spriteAnimation,  () => animationsRunning--));
        }

        while (animationsRunning > 0)
        {
            yield return null;
        }
    }

    IEnumerator PlaySpriteAnimation(ComicSpriteAnimation spriteAnimation, Action onFinish)
    {
        yield return spriteAnimation.PlayFrames();
        onFinish?.Invoke();
    }
}