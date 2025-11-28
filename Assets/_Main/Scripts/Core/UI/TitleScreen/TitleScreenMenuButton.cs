using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenMenuButton : MonoBehaviour
{
    public Image image;
    public Color selectedColor;
    public TitleScreenSubMenu menuToOpen;
    public AudioClip soundEffect;

    public void Click()
    {
        StartCoroutine(ClickAnimation());
    }

    private IEnumerator ClickAnimation()
    {
        image.DOKill();
        image.color = Color.black;

        image.DOColor(selectedColor, 0.05f)
            .SetLoops(6, LoopType.Yoyo)
            .SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.05f * 6f);
        
        TitleScreenMainMenu.instance.SwitchMenus(menuToOpen);
    }
    
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