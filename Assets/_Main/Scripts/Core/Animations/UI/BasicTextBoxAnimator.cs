
using DG.Tweening;
using DIALOGUE;
using UnityEngine;

public abstract class BasicTextBoxAnimator : MonoBehaviour
{
    public DialogueContainer dialogueContainer;
    
    public RectTransform namePlate;
    public RectTransform dialoguePart;
    protected CanvasGroup dialogueBoxCanvasGroup;
    protected CanvasGroup namePlateCanvasGroup;
    
    public bool textBoxVisible;
    public bool namePlateVisible;

    public CanvasGroup uiContainer;
    public CanvasGroup choicesPart;
    public bool uiEnabled = true;

    public RectTransform tabGuide;
    public RectTransform helpGuide;
    
    public abstract void TextBoxAppear();
    
    public abstract void TextBoxDisappear();
    
    public abstract void ShowNamePlate();

    public abstract void HideNamePlate();
    
    public virtual void Initialize()
    {
        DialogueSystem.instance.ClearTextBox();
        dialogueBoxCanvasGroup = dialoguePart.GetComponent<CanvasGroup>();
        namePlateCanvasGroup = namePlate.GetComponent<CanvasGroup>();
        
        dialogueBoxCanvasGroup.alpha = 0f;
        namePlateCanvasGroup.alpha = 0f;
        
        textBoxVisible = false;
        namePlateVisible = false;

    }

    public void ToggleUI()
    {
        if (uiContainer == null || choicesPart == null)
            return;
        uiEnabled = !uiEnabled;
        float opacity = uiEnabled ? 1f : 0f;
        uiContainer.alpha = opacity;
        choicesPart.alpha = opacity;
    }
}
