using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;

public class ComicPanel : MonoBehaviour
{
    [Serializable]
    public class ComicSound
    {
        public AudioClip soundEffect;
        public float delay;
    }
    
    private CanvasGroup canvasGroup;
    public List<ComicAnimatedSprite> spriteAnimations = new List<ComicAnimatedSprite>();
    public List<ComicSound> soundEffects = new List<ComicSound>();
    public List<DialogueNode> textBeforePanel = new();
    public List<DialogueNode> textAfterPanel = new();

    private Coroutine runningPanelCoroutine;
    private bool isDone;
    private bool isCancelled;

    public virtual void StartUpAnimation()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public IEnumerator Play()
    {
        runningPanelCoroutine = StartCoroutine(PlayPanel());

        yield return new WaitUntil(() => isDone);
    }

    private IEnumerator PlayPanel()
    {
        if (textBeforePanel.Count > 0)
        {
            DialogueSystem.instance.dialogueBoxAnimator.gameObject.SetActive(true);
            OverlayTextBoxManager.instance.Show();
        }
        foreach (DialogueNode node in textBeforePanel)
        {
            yield return DialogueSystem.instance.Say(node);
        }
        
        OverlayTextBoxManager.instance.Hide();
        
        yield return OnAppear();
        
        if(isCancelled)
            yield break;
        
        int animationsRunning = spriteAnimations.Count;
        foreach (ComicAnimatedSprite spriteAnimation in spriteAnimations)
        {
            StartCoroutine(PlaySpriteAnimation(spriteAnimation, () => animationsRunning--));
        }

        foreach (ComicSound sound in soundEffects)
        {
            StartCoroutine(PlayComicSound(sound));
        }

        while (animationsRunning > 0)
        {
            yield return null;
        }

        if (textAfterPanel.Count > 0)
        {
            OverlayTextBoxManager.instance.Show();
        }
        
        foreach (DialogueNode node in textAfterPanel)
        {
            yield return DialogueSystem.instance.Say(node);
        }

        isDone = true;
    }

    IEnumerator PlaySpriteAnimation(ComicAnimatedSprite animatedSprite, Action onFinish)
    {
        yield return animatedSprite.PlayFrames();
        onFinish?.Invoke();
    }

    protected virtual IEnumerator OnAppear()
    {
        canvasGroup.DOFade(1f, 0.1f);
        yield return null;
    }

    public virtual bool IsReady()
    {
        return true;
    }

    private IEnumerator PlayComicSound(ComicSound sound)
    {
        yield return new WaitForSeconds(sound.delay);
        SoundManager.instance.PlaySoundEffect(sound.soundEffect);
    }

    public void Stop()
    {
        if(runningPanelCoroutine != null)
           StopCoroutine(runningPanelCoroutine);
        isCancelled = true;
    }
}