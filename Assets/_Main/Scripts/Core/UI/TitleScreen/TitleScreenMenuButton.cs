using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class TitleScreenMenuButton : MonoBehaviour
{
    public Image image;
    public Color selectedColor;
    public AudioClip soundEffect;

    public virtual void Click()
    {
        ClickAnimation();
        SoundManager.instance.PlaySoundEffect(soundEffect);
    }

    private void ClickAnimation()
    {
        image.DOKill();
        image.color = Color.black;
        image.DOColor(selectedColor, 0.05f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.Linear).SetUpdate(true);
    }

    public void HoverButtonAnimation()
    {
        image.DOKill();
        image.color = Color.black;

        image.DOColor(selectedColor, 0.2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear).SetUpdate(true);
    }

    public void DisableHover()
    {
        image.DOKill();
        image.color = Color.black;
    }
}