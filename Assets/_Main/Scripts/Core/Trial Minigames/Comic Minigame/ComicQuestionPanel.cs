using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ComicPin
{
    public string pinName;
    public Sprite pinImage;
}
public class ComicQuestionPanel : ComicPanel, IDropHandler, IPointerClickHandler
{
    public ComicPin truePin;
    public ComicDraggablePin selectedPin;
    public CanvasGroup blueQuestionMarkOverlay;
    private RectTransform rectTransform;
    public Image questionMark;
    public Image glow;

    public List<DialogueNode> infoNodes;
    
    private Sequence glowSequence;

    public AudioClip pinAssignSound;

    public AudioClip correctSound;
    public AudioClip wrongSound;

    private ComicQuestionPanel originalPanel;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        glowSequence = DOTween.Sequence();
        AnimateGlow();
    }

    private void AnimateGlow()
    {
        glowSequence.Append(glow.rectTransform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo));
        glowSequence.AppendInterval(1f);
        glowSequence.SetLoops(-1);
        glowSequence.SetTarget(this);
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
            SoundManager.instance.PlaySoundEffect(correctSound);
            ComicManager.instance.LockPin();
            selectedPin.CorrectAnimation();
            yield return new WaitForSeconds(0.4f);
            blueQuestionMarkOverlay.DOFade(0f, 0.2f);
        }
        else
        {
            SoundManager.instance.PlaySoundEffect(wrongSound);
            ComicManager.instance.RemovePinFromPanel();
            selectedPin.WrongAnimation();
            yield return ComicManager.instance.WrongAnswer();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedPin == null)
        {
            StartCoroutine(SayHint());
        }
    }

    private IEnumerator SayHint()
    {
        DialogueSystem.instance.inputButton.gameObject.SetActive(true);
        ComicManager.instance.isInPuzzle = false;
        yield return ComicManager.instance.PlayComicNodes(infoNodes);
        ComicManager.instance.isInPuzzle = true;
        DialogueSystem.instance.inputButton.gameObject.SetActive(false);
    }

    public override bool IsReady()
    {
        return selectedPin != null;
    }
}
