using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public MenuScreen menuToOpen;
    public bool disabled;
    public MenuScreenContainer menuScreenContainer;
    public RectTransform rectTransform;
    public float originalPosY;
    public Image blueAura;
    public Image icon;

    void Start()
    {
        originalPosY = rectTransform.anchoredPosition.y;
        if (disabled)
        {
            icon.DOFade(0.3f, 0f).SetUpdate(true);
        }

        blueAura.DOFade(0f, 0f).SetUpdate(true);
    }

    public virtual void Click()
    {
        menuScreenContainer.OpenMenu(menuToOpen);
    }

    public void StartHover()
    {
        rectTransform.DOAnchorPosY(originalPosY + 40f, 0.2f).SetUpdate(true);
        blueAura.DOFade(1f, 0.2f).SetUpdate(true);
    }

    public void StopHover()
    {
        rectTransform.DOAnchorPosY(originalPosY, 0.2f).SetUpdate(true);
        blueAura.DOFade(0f, 0.2f).SetUpdate(true);
    }
}