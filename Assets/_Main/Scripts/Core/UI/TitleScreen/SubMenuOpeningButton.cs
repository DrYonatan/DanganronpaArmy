using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SubMenuOpeningButton : TitleScreenMenuButton
{
    public TitleScreenSubMenu menuToOpen;
    
    public override void Click()
    {
        base.Click();
        ClickAnimation();
    }
    
    private void ClickAnimation()
    {
        image.DOKill();
        image.color = Color.black;

        image.DOColor(selectedColor, 0.05f)
            .SetLoops(6, LoopType.Yoyo)
            .SetEase(Ease.Linear);
        
        TitleScreenMainMenu.instance.SwitchMenus(menuToOpen);
    }
}
