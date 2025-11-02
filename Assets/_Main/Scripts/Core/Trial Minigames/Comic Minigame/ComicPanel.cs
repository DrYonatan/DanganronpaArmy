using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;
using UnityEngine.UI;

public class ComicPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public List<ComicSpriteAnimation> spriteAnimations = new List<ComicSpriteAnimation>();
    public List<DialogueNode> textBeforePanel = new();
    public List<DialogueNode> textAfterPanel = new();

    public virtual void StartUpAnimation()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public IEnumerator Play()
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
        
        int animationsRunning = spriteAnimations.Count;
        foreach (ComicSpriteAnimation spriteAnimation in spriteAnimations)
        {
            StartCoroutine(PlaySpriteAnimation(spriteAnimation, () => animationsRunning--));
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
    }

    IEnumerator PlaySpriteAnimation(ComicSpriteAnimation spriteAnimation, Action onFinish)
    {
        yield return spriteAnimation.PlayFrames();
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
    
    
}