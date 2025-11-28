using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SubMenuOpeningButton : TitleScreenMenuButton
{
    public TitleScreenSubMenu menuToOpen;
    
    public override void Click()
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
}
