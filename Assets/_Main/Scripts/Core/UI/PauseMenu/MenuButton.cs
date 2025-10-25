using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public GameObject menuToOpen;
    private bool isHovered = false;
    public Image buttonTexture;


    void Update()
    {
        buttonTexture.color = isHovered ? Color.red : Color.clear;
    }
    public void Click()
    {
        menuToOpen.SetActive(true);
    }

    public void SetIsHovered(bool isHovered)
    {
        this.isHovered = isHovered;
    }
}
