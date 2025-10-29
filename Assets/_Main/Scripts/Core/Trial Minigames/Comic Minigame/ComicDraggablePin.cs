using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComicDraggablePin : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public ComicPin pin;
    public Image image;
    private RectTransform rectTransform;
    private Canvas canvas;

    public RectTransform parent;
    public ComicQuestionPanel assignedPanel;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetPin(ComicPin pin)
    {
        this.pin = pin;
        image.sprite = pin.pinImage;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ComicManager.instance.animator.currentDraggedPin = this;
        TrialCursorManager.instance.Hide();
        rectTransform.localScale *= 1.5f;
        rectTransform.SetParent(parent.parent.parent);
        ComicManager.instance.animator.LowerPinsContainer();
        GetComponent<Image>().raycastTarget = false;
        if(assignedPanel)
            assignedPanel.selectedPin = null;
        assignedPanel = null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = true;
        rectTransform.localScale /= 1.5f;
        TrialCursorManager.instance.Show();
        ComicManager.instance.animator.currentDraggedPin = null;
        ComicManager.instance.animator.RaisePinsContainer();

        if (assignedPanel == null)
        {
            rectTransform.SetParent(parent);
            rectTransform.localPosition = Vector3.zero;
        }
    }
}