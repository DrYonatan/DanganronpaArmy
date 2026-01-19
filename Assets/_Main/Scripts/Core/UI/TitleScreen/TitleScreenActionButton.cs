using DG.Tweening;
using UnityEngine.UI;

public class TitleScreenActionButton : TitleScreenMenuButton
{
    public Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public override void Click()
    {
        base.Click();
        button.onClick.Invoke();
    }
}
