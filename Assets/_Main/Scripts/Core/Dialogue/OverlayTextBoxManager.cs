using DG.Tweening;
using DIALOGUE;
using UnityEngine;

public class OverlayTextBoxManager : MonoBehaviour
{
    public static OverlayTextBoxManager instance { get; private set; }

    public CanvasGroup canvasGroup;
    public DialogueContainer dialogueContainer;

    void Awake()
    {
        instance = this;
    }

    public void SetAsTextBox()
    {
        DialogueSystem.instance.SetTextBox(dialogueContainer);
    }
    
    public void Show()
    {
        canvasGroup.DOFade(1f, 0.2f);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0f, 0.2f);
    }
    
}
