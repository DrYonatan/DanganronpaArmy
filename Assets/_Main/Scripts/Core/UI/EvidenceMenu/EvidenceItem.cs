using TMPro;
using UnityEngine;

public class EvidenceItem : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
