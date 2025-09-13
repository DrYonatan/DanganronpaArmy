using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangmanLetterBlock : MonoBehaviour
{
    public Image frame0;
    public Image frame1;
    public Image glow;
    public Image questionBlock;
    public Image questionMark;
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;
    public char letterRepresented;

    private bool isAquired;
    
    void Start()
    {
        questionMark.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void TurnIntoCurrentLetter()
    {
        questionBlock.DOFade(1f, 0.5f);
        EmitFrames();
    }
    
    public void GetAquired()
    {
        questionMark.DOKill(); // Stop the blinking set in the Start
        text.text = letterRepresented.ToString();
        text.alpha = 0f;
        isAquired = true;
        Sequence seq = DOTween.Sequence();

        seq.Append(glow.DOFade(1f, 0.2f));
        seq.Append(questionMark.DOFade(0f, 0f));
        seq.Append(questionBlock.DOFade(0f, 0f));
        seq.Append(glow.DOFade(0f, 0.2f));

        seq.Append(text.DOFade(1f, 0.2f));
    }
    
    void EmitFrames()
    {
        // Reset both frames
        frame0.color = new Color(frame0.color.r, frame0.color.g, frame0.color.b, 0);
        frame1.color = new Color(frame1.color.r, frame1.color.g, frame1.color.b, 0);
        frame0.transform.localScale = Vector3.one;
        frame1.transform.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();

        // Frame0 anim
        seq.Append(frame0.DOFade(1f, 0.2f)); // fade in
        seq.Join(frame0.transform.DOScale(2f, 0.5f)); // scale up
        seq.Join(frame0.DOFade(0f, 0.5f).SetDelay(0.1f)); // fade out overlapping with scale

        // Frame1 anim, starts halfway through frame0’s growth
        seq.Insert(0.5f, frame1.DOFade(1f, 0.2f));
        seq.Insert(0.5f, frame1.transform.DOScale(2f, 0.5f));
        seq.Insert(0.7f, frame1.DOFade(0f, 0.5f));

        // Wait 1 second after both done
        seq.AppendInterval(1f);
        
        seq.SetLoops(-1);

        seq.OnStepComplete(() =>
        {
            if (isAquired)
                seq.Kill();
        });
        
    }
    
}
