using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour
{
    public Image highlightIcon;
    public Image characterIcon;
    
    public void StartHover()
    {
        highlightIcon.color = new Color(highlightIcon.color.r, highlightIcon.color.g, highlightIcon.color.b, 1f);
        highlightIcon
            .DOFade(0.4f, 0.4f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public void StopHover()
    {
        highlightIcon.DOKill();
        highlightIcon.color = new Color(highlightIcon.color.r, highlightIcon.color.g, highlightIcon.color.b, 0f);
    }

    public void SetIcon(Sprite icon)
    {
        characterIcon.sprite = icon;
    }
}