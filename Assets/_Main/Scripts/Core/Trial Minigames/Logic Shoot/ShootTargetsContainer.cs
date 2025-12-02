using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootTargetsContainer : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI questionText;
    public List<ShootTarget> targets;

    public void GenerateTargetContainer()
    {
        foreach (ShootTarget target in targets)
        {
            
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform rt = GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Spawn the marker inside the object
        RectTransform hole = Instantiate(LogicShootManager.instance.animator.holePrefab, rt);
        hole.anchoredPosition = localPoint;
    }
}
