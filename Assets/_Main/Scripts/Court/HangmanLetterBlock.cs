using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangmanLetterBlock : MonoBehaviour
{
    public Image image;
    public Image glow;
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;
    public char letterRepresented;

    public void GetAquired()
    {
        text.DOFade(0, 0.1f)
            .OnComplete(() => text.text = letterRepresented.ToString());
        Sequence seq = DOTween.Sequence();
        
        seq.Append(glow.DOFade(1f, 0.2f).SetLoops(2, LoopType.Yoyo));
        

        seq.Append(text.DOFade(1f, 0.2f));
    }
    
}
