using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootTarget : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI questionText;
    public List<ShootTargetArea> areas;
    public float timeOut;

    public Vector2 targetPosition;

    private bool isDisappearing;
    
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void LifeTime()
    { 
       StartCoroutine(LifeTimeRoutine());
    }

    private IEnumerator LifeTimeRoutine()
    {
        rectTransform.DOAnchorPos(targetPosition, timeOut);
        yield return new WaitForSeconds(timeOut);
        if(!isDisappearing)
           Destroy(gameObject);
    }

    public void DisappearAnimation()
    {
        rectTransform.DOKill();
        isDisappearing = true;
        rectTransform.DOAnchorPosY(-1000, 0.5f).OnComplete(() => Destroy(gameObject));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (LogicShootManager.instance.coolDown)
            return;
        
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Spawn the marker inside the object
        RectTransform hole = Instantiate(LogicShootManager.instance.animator.holePrefab, rectTransform);
        hole.anchoredPosition = localPoint;
    }
}