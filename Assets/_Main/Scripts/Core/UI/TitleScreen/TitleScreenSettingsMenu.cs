using DG.Tweening;

public class TitleScreenSettingsMenu : TitleScreenSubMenu
{
    public VolumeSlidersMenu volumeSlidersMenu;

    void Update()
    {
        
    }

    public override bool CanExit()
    {
        return !volumeSlidersMenu.isConcentrating;
    }

    public override void OutroAnimation()
    {
        volumeSlidersMenu.transform.DOKill();
        volumeSlidersMenu.transform.DOScaleX(0f, 0.2f);
    }

    public override void AppearAnimation()
    {
        volumeSlidersMenu.transform.DOKill();
        volumeSlidersMenu.transform.DOScaleX(1f, 0f);
    }
}
