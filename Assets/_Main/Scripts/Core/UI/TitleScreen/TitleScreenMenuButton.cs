using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class TitleScreenMenuButton : MonoBehaviour
{
    public Image image;
    public Color selectedColor;
    public AudioClip soundEffect;

    public abstract void Click();

    public void HoverButtonAnimation()
    {
        image.DOKill();
        image.color = Color.black;
        
        image.DOColor(Color.red, 0.2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    public void DisableHover()
    {
        image.DOKill();
        image.color = Color.black;
    }
}