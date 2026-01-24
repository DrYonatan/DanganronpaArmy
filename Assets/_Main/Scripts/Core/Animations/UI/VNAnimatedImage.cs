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
        foreach (ComicAnimatedSprite spriteAnimation in animationSegments[currentAnimationIndex].sprites)
        {
            yield return spriteAnimation.PlayFrames();
        }

        currentAnimationIndex++;
    }
}