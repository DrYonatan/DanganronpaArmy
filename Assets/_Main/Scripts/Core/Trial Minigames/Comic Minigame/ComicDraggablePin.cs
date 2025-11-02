using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComicDraggablePin : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler,
    IPointerExitHandler
{
    public ComicPin pin;
    public Image mask;
    public Image image;
    public Image outline;
    public Image glow;
    public Image underlayRing;
    public Image overlayRing;
    private RectTransform rectTransform;
    private Canvas canvas;

    public AudioClip dragSound;

    public RectTransform parent;
    public ComicQuestionPanel assignedPanel;

    private bool isDragged;

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
        isDragged = true;
        SoundManager.instance.PlaySoundEffect(dragSound);
        ComicManager.instance.animator.currentDraggedPin = this;
        mask.raycastTarget = false;
        TrialCursorManager.instance.Hide();
        rectTransform.localScale *= 1.5f;
        rectTransform.SetParent(parent.parent.parent);
        ComicManager.instance.animator.LowerPinsContainer();
        if (assignedPanel)
        {
            assignedPanel.selectedPin = null;
            StartGlowing();
        }
        assignedPanel = null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragged = false;
        StopGlowing();
        mask.raycastTarget = true;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (assignedPanel == null)
        {
            StartGlowing();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isDragged)
            StopGlowing();
    }

    private void StartGlowing()
    {
        glow.DOFade(1f, 0.1f);
        glow.rectTransform.localScale = Vector3.one;
        glow.rectTransform.DOScale(1.1f, 0.2f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopGlowing()
    {
        glow.DOFade(0f, 0.1f).OnComplete(() => glow.DOKill()).OnComplete(() => glow.rectTransform.DOKill());
    }

    public void CorrectAnimation()
    {
        float fullOpacity = 0.7f;
        float duration = 0.1f;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(overlayRing.DOFade(fullOpacity, 0.05f)
            .SetLoops(4, LoopType.Yoyo));
        seq.Append(overlayRing.DOFade(fullOpacity, 0f));
        seq.Append(overlayRing.rectTransform.DOScale(1.2f, duration));
        seq.Join(overlayRing.DOFade(0f, duration));
        seq.Join(underlayRing.DOFade(fullOpacity, duration * 0.8f).SetDelay(duration / 2));
        seq.Append(underlayRing.DOFade(0f, duration));
        seq.Join(underlayRing.rectTransform.DOScale(1.2f, duration));
    }
}