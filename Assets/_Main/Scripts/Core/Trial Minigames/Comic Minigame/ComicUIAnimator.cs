using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ComicUIAnimator : MonoBehaviour
{
    public RectTransform questionPanelsSpawnLocation;

    public CanvasGroup puzzleCanvasGroup;
    public CanvasGroup solutionCanvasGroup;

    public RectTransform solutionPagesContainer;

    public RectTransform puzzlePagesContainer;
    public RectTransform pinsContainer;

    public ComicDraggablePin pinPrefab;

    public List<ComicPage> pageObjects = new List<ComicPage>();
    public List<ComicDraggablePin> draggablePins = new List<ComicDraggablePin>();

    private Vector2 pinsContainerOriginalPos;

    public ComicDraggablePin currentDraggedPin;

    public RectTransform movingMist;
    public RectTransform frontMist;

    private Tween beatTween;

    public void GeneratePuzzlePages(List<ComicPage> pages)
    {
        foreach (ComicPage page in pages)
        {
            ComicPage instantiated = Instantiate(page, puzzlePagesContainer);
            instantiated.transform.localScale *= 0.7f;
            pageObjects.Add(instantiated);
        }
    }

    public void GenerateComicPins(List<ComicPin> pins)
    {
        foreach (ComicPin pin in pins)
        {
            GameObject parent = GeneratePinParent();
            ComicDraggablePin draggablePin = Instantiate(pinPrefab, parent.transform);
            draggablePin.SetPin(pin);
            draggablePin.parent = parent.GetComponent<RectTransform>();
            draggablePin.transform.localPosition = Vector3.zero;

            draggablePins.Add(draggablePin);
        }
    }

    public ComicPage GenerateSolutionPage(int index)
    {
        ComicPage newPage = Instantiate(pageObjects[index], solutionPagesContainer);
        newPage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        newPage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        newPage.rectTransform.sizeDelta = new Vector2(1920, 1080);
        newPage.transform.localScale = Vector3.one;
        newPage.transform.localPosition = Vector3.zero;

        return newPage;
    }

    private GameObject GeneratePinParent()
    {
        GameObject parent = new GameObject("PinContainer");
        parent.AddComponent<RectTransform>();
        parent.transform.SetParent(pinsContainer);
        parent.transform.localScale = Vector3.one;
        parent.transform.localPosition = Vector3.zero;
        parent.transform.localRotation = Quaternion.Euler(Vector3.zero);
        return parent;
    }

    public void ShowPuzzleUI()
    {
        puzzleCanvasGroup.alpha = 1f;
        solutionCanvasGroup.alpha = 0f;
        pinsContainerOriginalPos = pinsContainer.anchoredPosition;
        AnimatePuzzleBackground();
    }

    public void ShowSolutionUI()
    {
        puzzleCanvasGroup.alpha = 0f;
        solutionCanvasGroup.alpha = 1f;
        StopAnimatingPuzzleBackground();
    }

    public void LowerPinsContainer()
    {
        pinsContainer.DOAnchorPosY(pinsContainerOriginalPos.y - 400, 0.2f);
    }

    public void RaisePinsContainer()
    {
        pinsContainer.DOAnchorPosY(pinsContainerOriginalPos.y, 0.2f);
    }

    private void AnimatePuzzleBackground()
    {
        Vector2 originalPos = movingMist.anchoredPosition;

        movingMist.DOAnchorPosX(originalPos.x + 1000, 10f).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
        
        StartBeating();
        
    }

    private void StopAnimatingPuzzleBackground()
    {
        movingMist.DOKill();
        beatTween.Kill();
    }
    
    private void StartBeating()
    {
        Sequence beatSequence = DOTween.Sequence()
            .Append(frontMist.DOScale(1.2f, 0.05f).SetEase(Ease.OutQuad))
            .Append(frontMist.DOScale(1f, 0.05f).SetEase(Ease.InQuad))
            .AppendInterval(2f)
            .SetLoops(-1, LoopType.Restart);

        beatTween = beatSequence;
    }
}