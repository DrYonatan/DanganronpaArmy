using System;
using UnityEngine;
using UnityEngine.UI;

public class UILine : MonoBehaviour
{
    private Vector2 pos1;
    private Vector2 pos2;
    public Image image;
    public RectTransform rectTransform;

    public void SetPositions(Vector2 one, Vector2 two)
    {
        pos1 = one;
        pos2 = two;

        Vector2 aux;
        if (pos1.x > pos2.x)
        {
            aux = pos1;
            pos1 = pos2;
            pos2 = aux;
        }
        
        rectTransform.anchoredPosition = (pos1 + pos2) / 2;
        Vector3 dif = pos2 - pos1;
        rectTransform.sizeDelta = new Vector3(dif.magnitude, 5);
        rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));

    }
}