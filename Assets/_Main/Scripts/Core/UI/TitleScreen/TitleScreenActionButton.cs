using DG.Tweening;
using UnityEngine.UI;

public class TitleScreenActionButton : TitleScreenMenuButton
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public override void Click()
    {
        image.DOKill();
        button.onClick.Invoke();
    }
}
