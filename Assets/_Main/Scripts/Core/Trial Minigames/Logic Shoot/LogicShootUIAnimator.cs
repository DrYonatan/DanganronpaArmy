using TMPro;
using UnityEngine;

public class LogicShootUIAnimator : MonoBehaviour
{
    public TextMeshProUGUI mikbazText;
    public RectTransform holePrefab;

    public void ShowMikbazText(int number)
    {
        mikbazText.text = "מקבץ " +  number + " !!";
    }
}
