using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteAnimationSegment
{
    public List<ComicAnimatedSprite> sprites;
    public List<ComicPanel.ComicSound> soundEffects = new List<ComicPanel.ComicSound>();

}

public class VNAnimatedImage : MonoBehaviour
{
    public int currentAnimationIndex;
    public bool isCutscene; // If it's a cutscene image, the behaviour changes slightly to make the entire UI disappear and autoplay
    public List<SpriteAnimationSegment> animationSegments;

    public IEnumerator ForwardSegment()
    {
        int animationsRunning = animationSegments[currentAnimationIndex].sprites.Count;
        foreach (ComicAnimatedSprite spriteAnimation in animationSegments[currentAnimationIndex].sprites)
        {
            StartCoroutine(PlaySpriteAnimation(spriteAnimation,() => animationsRunning--));
        }

        foreach (ComicPanel.ComicSound comicSound in animationSegments[currentAnimationIndex].soundEffects)
        {
            StartCoroutine(PlayComicSound(comicSound));
        }

        while (animationsRunning > 0)
        {
            yield return null;
        }

        currentAnimationIndex++;
    }
    
    private IEnumerator PlayComicSound(ComicPanel.ComicSound sound)
    {
        yield return new WaitForSeconds(sound.delay);
        SoundManager.instance.PlaySoundEffect(sound.soundEffect);
    }
    
    IEnumerator PlaySpriteAnimation(ComicAnimatedSprite animatedSprite, Action onFinish)
    {
        yield return animatedSprite.PlayFrames();
        onFinish?.Invoke();
    }
}