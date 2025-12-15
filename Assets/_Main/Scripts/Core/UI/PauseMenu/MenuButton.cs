using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public MenuScreen menuToOpen;
    private bool isHovered = false;
    public bool disabled;
    public Image buttonTexture;
    public MenuScreenContainer menuScreenContainer;

    void Update()
    {
        buttonTexture.color = isHovered && !disabled ? Color.red : Color.clear;
    }
    public virtual void Click()
    {
        if (!disabled)
        {
            menuScreenContainer.OpenMenu(menuToOpen);
        }
    }

    public void SetIsHovered(bool isHovered)
    {
        this.isHovered = isHovered;
    }
}
