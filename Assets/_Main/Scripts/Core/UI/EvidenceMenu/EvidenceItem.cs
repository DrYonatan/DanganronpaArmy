using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceItem : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image background;
    public bool isHovered;
    public Color hoverColor;

    public void SetText(string text)
    {
        this.text.text = text;
    }

    void Update()
    {
        background.color = isHovered ? hoverColor : new Color(0, 0, 0, 0);
    }
}
