using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public MenuScreen menuToOpen;
    private bool isHovered = false;
    public Image buttonTexture;
    public MenuScreenContainer menuScreenContainer;

    void Update()
    {
        buttonTexture.color = isHovered ? Color.red : Color.clear;
    }
    public void Click()
    {
        menuScreenContainer.OpenMenu(menuToOpen);
    }

    public void SetIsHovered(bool isHovered)
    {
        this.isHovered = isHovered;
    }
}
