using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ComicPin
{
    public string pinName;
    public Sprite pinImage;
}
public class ComicQuestionPanel : ComicPanel, IDropHandler
{
    public ComicPin truePin;
    public ComicDraggablePin selectedPin;
    public CanvasGroup blueQuestionMarkOverlay;
    private RectTransform rectTransform;
    public Image questionMark;

    public AudioClip pinAssignSound;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        selectedPin = null;
    }

    public override void StartUpAnimation()
    {
        blueQuestionMarkOverlay.DOFade(1f, 0f);
        Vector2 originalPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = ComicManager.instance.animator.questionPanelsSpawnLocation.anchoredPosition;
        rectTransform.localRotation = Quaternion.Euler(0, 0, -20);
        rectTransform.DOLocalRotate(Vector3.zero, 0.2f);
        rectTransform.DOAnchorPos(originalPos, 0.2f);
    }

    protected override IEnumerator OnAppear()
    {
        selectedPin = GetComponentInChildren<ComicDraggablePin>();
        if (selectedPin.pin.pinName.Equals(truePin.pinName))
        {
            selectedPin.CorrectAnimation();
            yield return new WaitForSeconds(0.4f);
            blueQuestionMarkOverlay.DOFade(0f, 0.2f);
        }
        else
        {
            selectedPin.WrongAnimation();
            ComicManager.instance.WrongAnswer();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (selectedPin == null)
        {
            SoundManager.instance.PlaySoundEffect(pinAssignSound);
            selectedPin = ComicManager.instance.animator.currentDraggedPin;
            selectedPin.transform.SetParent(blueQuestionMarkOverlay.transform);
            selectedPin.transform.localPosition = questionMark.transform.localPosition;
            selectedPin.assignedPanel = this;
            ComicManager.instance.UpdateIsReadyToPresent();
        }
    }

    public override bool IsReady()
    {
        return selectedPin != null;
    }
}
