using DG.Tweening;
using UnityEngine;

public class QuestionBubble: MonoBehaviour
{
    public CanvasGroup canvasGroup;
    
    public void Open()
    {
        gameObject.SetActive(true);
    }
    
    void OnEnable()
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, 0.3f);
    }

    public void Close()
    {
        canvasGroup.DOFade(0f, 0.3f).OnComplete(() => { gameObject.SetActive(false); });
    }
}