using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class SpriteAnimationKeyFrame<T>
{
    public T attribute;
    public float duration;
}

[Serializable]
public class SpriteAnimation<T>
{
    public int repeatTimes = 1;
    public List<SpriteAnimationKeyFrame<T>> frames = new();
}

public class ComicAnimatedSprite : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image image;
    public SpriteAnimation<Vector2> positionAnimation = new();
    public SpriteAnimation<float> rotationAnimation = new();
    public SpriteAnimation<Vector3> scaleAnimation = new();
    public SpriteAnimation<Color> colorAnimation = new();

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public IEnumerator PlayFrames()
    {
        int running = 4;

        StartCoroutine(AnimatePosition(positionAnimation, () => running--));

        StartCoroutine(AnimateRotation(rotationAnimation, () => running--));

        StartCoroutine(AnimateScale(scaleAnimation, () => running--));

        StartCoroutine(AnimateColor(colorAnimation, () => running--));

        while (running > 0)
            yield return null;
    }

    IEnumerator AnimatePosition(SpriteAnimation<Vector2> spriteAnimation, Action onFinish)
    {
        if (spriteAnimation.repeatTimes == -1)
            onFinish?.Invoke();

        int repeat = 0;
        while (repeat < spriteAnimation.repeatTimes || spriteAnimation.repeatTimes == -1)
        {
            for (int i = 0; i < spriteAnimation.frames.Count - 1; i++)
            {
                SpriteAnimationKeyFrame<Vector2> startKeyFrame = spriteAnimation.frames[i];
                SpriteAnimationKeyFrame<Vector2> endKeyFrame = spriteAnimation.frames[i + 1];

                rectTransform.anchoredPosition = startKeyFrame.attribute;

                rectTransform.DOAnchorPos(endKeyFrame.attribute, endKeyFrame.duration)
                    .SetTarget(ComicManager.instance.currentPresentedPage);

                yield return new WaitForSeconds(endKeyFrame.duration);
            }

            repeat++;
        }


        onFinish?.Invoke();
    }

    IEnumerator AnimateRotation(SpriteAnimation<float> spriteAnimation, Action onFinish)
    {
        if (spriteAnimation.repeatTimes == -1)
            onFinish?.Invoke();

        int repeat = 0;
        while (repeat < spriteAnimation.repeatTimes || spriteAnimation.repeatTimes == -1)
        {
            for (int i = 0; i < spriteAnimation.frames.Count - 1; i++)
            {
                SpriteAnimationKeyFrame<float> startKeyFrame = spriteAnimation.frames[i];
                SpriteAnimationKeyFrame<float> endKeyFrame = spriteAnimation.frames[i + 1];

                rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, startKeyFrame.attribute));

                rectTransform.DOLocalRotate(new Vector3(0, 0, endKeyFrame.attribute), endKeyFrame.duration)
                    .SetTarget(ComicManager.instance.currentPresentedPage);
                ;

                yield return new WaitForSeconds(endKeyFrame.duration);
            }

            repeat++;
        }

        onFinish?.Invoke();
    }

    IEnumerator AnimateScale(SpriteAnimation<Vector3> spriteAnimation, Action onFinish)
    {
        if (spriteAnimation.repeatTimes == -1)
            onFinish?.Invoke();

        int repeat = 0;

        while (repeat < spriteAnimation.repeatTimes || spriteAnimation.repeatTimes == -1)
        {
            for (int i = 0; i < spriteAnimation.frames.Count - 1; i++)
            {
                SpriteAnimationKeyFrame<Vector3> startKeyFrame = spriteAnimation.frames[i];
                SpriteAnimationKeyFrame<Vector3> endKeyFrame = spriteAnimation.frames[i + 1];

                rectTransform.localScale = startKeyFrame.attribute;

                rectTransform.DOScale(endKeyFrame.attribute, endKeyFrame.duration)
                    .SetTarget(ComicManager.instance.currentPresentedPage);
                ;

                yield return new WaitForSeconds(endKeyFrame.duration);
            }

            repeat++;
        }


        onFinish?.Invoke();
    }

    IEnumerator AnimateColor(SpriteAnimation<Color> spriteAnimation, Action onFinish)
    {
        if (spriteAnimation.repeatTimes == -1)
            onFinish?.Invoke();

        int repeat = 0;

        while (repeat < spriteAnimation.repeatTimes || spriteAnimation.repeatTimes == -1)
        {
            for (int i = 0; i < spriteAnimation.frames.Count - 1; i++)
            {
                SpriteAnimationKeyFrame<Color> startKeyFrame = spriteAnimation.frames[i];
                SpriteAnimationKeyFrame<Color> endKeyFrame = spriteAnimation.frames[i + 1];

                image.color = startKeyFrame.attribute;

                image.DOColor(endKeyFrame.attribute, endKeyFrame.duration)
                    .SetTarget(ComicManager.instance.currentPresentedPage);
                ;

                yield return new WaitForSeconds(endKeyFrame.duration);
            }

            repeat++;
        }

        onFinish?.Invoke();
    }
}