using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangmanLetterBlock : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;
    public char letterRepresented;

    public void GetAquired()
    {
        text.text = letterRepresented.ToString();
        text.DOFade(0, 0);
        text.DOFade(1f, 1f);
    }
    
}
