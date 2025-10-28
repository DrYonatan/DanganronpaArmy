using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComicDraggablePin : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private string pinName;
    public Image image;
    private RectTransform rectTransform;
    private Canvas canvas;

    private RectTransform parent;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetPin(ComicPin pin)
    {
        pinName = pin.pinName;
        image.sprite = pin.pinImage;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        TrialCursorManager.instance.Hide();
        rectTransform.localScale *= 1.5f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TrialCursorManager.instance.Show();
        rectTransform.localScale /= 1.5f;
        rectTransform.anchoredPosition = Vector2.zero;
    }
}