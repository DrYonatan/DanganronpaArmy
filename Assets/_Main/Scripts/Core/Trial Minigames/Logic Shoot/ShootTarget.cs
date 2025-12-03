using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootTarget : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI questionText;
    public List<ShootTargetArea> areas;
    public float timeOut;
    
    public IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(timeOut);
        Destroy(gameObject);
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
