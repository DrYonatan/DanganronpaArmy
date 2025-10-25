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

public class ComicSpriteAnimation : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image image;
    public List<SpriteAnimationKeyFrame<Vector2>> positionKeyFrames = new();
    public List<SpriteAnimationKeyFrame<float>> rotationKeyFrames = new();
    public List<SpriteAnimationKeyFrame<Vector3>> scaleKeyFrames = new();
    public List<SpriteAnimationKeyFrame<Color>> colorKeyFrames = new();

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public IEnumerator PlayFrames()
    {
        int running = 4;
        
        StartCoroutine(AnimatePosition(positionKeyFrames, () => running--));
        
        StartCoroutine(AnimateRotation(rotationKeyFrames, () => running--));

        StartCoroutine(AnimateScale(scaleKeyFrames, () => running--));
        
        StartCoroutine(AnimateColor(colorKeyFrames, () => running--));
        
        while (running > 0)
           yield return null;
    }

    IEnumerator AnimatePosition(List<SpriteAnimationKeyFrame<Vector2>> frames, Action onFinish)
    {
        for (int i = 0; i < frames.Count-1; i++)
        {
            SpriteAnimationKeyFrame<Vector2> startKeyFrame = frames[i];
            SpriteAnimationKeyFrame<Vector2> endKeyFrame = frames[i+1];
            
            rectTransform.anchoredPosition = startKeyFrame.attribute;

            rectTransform.DOAnchorPos(endKeyFrame.attribute, endKeyFrame.duration);
            
            yield return new WaitForSeconds(endKeyFrame.duration);
        }
        
        onFinish?.Invoke();
    }

    IEnumerator AnimateRotation(List<SpriteAnimationKeyFrame<float>> frames, Action onFinish)
    {
        for (int i = 0; i < frames.Count - 1; i++)
        {
            SpriteAnimationKeyFrame<float> startKeyFrame = frames[i];
            SpriteAnimationKeyFrame<float> endKeyFrame = frames[i+1];
            
            rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, startKeyFrame.attribute));

            rectTransform.DOLocalRotate(new Vector3(0, 0, endKeyFrame.attribute), endKeyFrame.duration);

            yield return new WaitForSeconds(endKeyFrame.duration);
        }
        onFinish?.Invoke();
    }

    IEnumerator AnimateScale(List<SpriteAnimationKeyFrame<Vector3>> frames, Action onFinish)
    {
        for (int i = 0; i < frames.Count - 1; i++)
        {
            SpriteAnimationKeyFrame<Vector3> startKeyFrame = frames[i];
            SpriteAnimationKeyFrame<Vector3> endKeyFrame = frames[i+1];
            
            rectTransform.localScale = startKeyFrame.attribute;

            rectTransform.DOScale(endKeyFrame.attribute, endKeyFrame.duration);

            yield return new WaitForSeconds(endKeyFrame.duration);
        }
        onFinish?.Invoke();
    }

    IEnumerator AnimateColor(List<SpriteAnimationKeyFrame<Color>> frames, Action onFinish)
    {
        for (int i = 0; i < frames.Count - 1; i++)
        {
            SpriteAnimationKeyFrame<Color> startKeyFrame = frames[i];
            SpriteAnimationKeyFrame<Color> endKeyFrame = frames[i+1];
            
            image.color = startKeyFrame.attribute;

            image.DOColor(endKeyFrame.attribute, endKeyFrame.duration);

            yield return new WaitForSeconds(endKeyFrame.duration);
        }
        onFinish?.Invoke();
    }
}