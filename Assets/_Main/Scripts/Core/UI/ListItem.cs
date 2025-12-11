using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image background;
    public bool isHovered;
    public Color hoverColor;
    private Tween blinkTween;

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void SetHovered(bool hovered)
    {
        if (isHovered == hovered) return;
        isHovered = hovered;

        if (hovered)
        {
            StartBlink();
        }
        else
        {
            StopBlink();
        }
    }
    
    private void StartBlink()
    {
        blinkTween?.Kill();

        background.color = hoverColor;

        blinkTween = background
            .DOColor(new Color(hoverColor.r, hoverColor.g, hoverColor.b, 0.2f), 0.5f)      // fade in
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine).SetUpdate(true);
    }

    private void StopBlink()
    {
        blinkTween?.Kill();
        background.color = new Color(0, 0, 0, 0);
    }
}