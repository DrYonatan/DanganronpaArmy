using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteAnimationSegment
{
    public List<ComicAnimatedSprite> sprites;
}

public class VNAnimatedImage : MonoBehaviour
{
    public int currentAnimationIndex;
    public List<SpriteAnimationSegment> animationSegments;

    public IEnumerator ForwardSegment()
    {
        int animationsRunning = animationSegments[currentAnimationIndex].sprites.Count;
        foreach (ComicAnimatedSprite spriteAnimation in animationSegments[currentAnimationIndex].sprites)
        {
            StartCoroutine(PlaySpriteAnimation(spriteAnimation,() => animationsRunning--));
        }

        while (animationsRunning > 0)
        {
            yield return null;
        }

        currentAnimationIndex++;
    }
    
    IEnumerator PlaySpriteAnimation(ComicAnimatedSprite animatedSprite, Action onFinish)
    {
        yield return animatedSprite.PlayFrames();
        onFinish?.Invoke();
    }
}