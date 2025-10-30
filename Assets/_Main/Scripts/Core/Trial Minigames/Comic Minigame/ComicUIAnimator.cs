using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComicUIAnimator : MonoBehaviour
{
    public RectTransform questionPanelsSpawnLocation;

    public CanvasGroup puzzleCanvasGroup;
    public CanvasGroup solutionCanvasGroup;

    public RectTransform solutionPagesContainer;

    public RectTransform puzzlePagesContainer;
    public RectTransform pinsContainer;

    public TextMeshProUGUI pagesCount;

    public ComicDraggablePin pinPrefab;

    public List<ComicPage> pageObjects = new List<ComicPage>();
    public List<ComicDraggablePin> draggablePins = new List<ComicDraggablePin>();

    private Vector2 pinsContainerOriginalPos;

    public ComicDraggablePin currentDraggedPin;

    public Image movingMist;
    public RectTransform frontMist;

    private Tween beatTween;
    
    public float pagesContainerStartPos = -900;
    public float pageWidth = 440;

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

            ComicDraggablePin shadowPin = Instantiate(draggablePin, parent.transform);
            shadowPin.transform.localPosition = Vector3.zero;
            shadowPin.image.color = new Color(0.2f, 0.2f, 0.2f);
            shadowPin.outline.color = new Color(0.2f, 0.2f, 0.2f);
            
            shadowPin.transform.SetAsFirstSibling();
            
            Destroy(shadowPin); // Destroys only the Draggable Pin Component, not the entire object

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
        UpdatePageNumber();
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
        Vector2 originalPos = movingMist.rectTransform.anchoredPosition;

        movingMist.rectTransform.DOAnchorPosX(-originalPos.x, 8f).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        Color color = movingMist.color;
        color.a = 0f;
        movingMist.color = color;
        
        movingMist.DOFade(1f, 4f).SetLoops(-1, LoopType.Yoyo);
        
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

    public void ScrollPuzzlePagesContainer(float amount)
    {
        puzzlePagesContainer.localPosition += new Vector3(amount, 0, 0);
        UpdatePageNumber();
    }
    
    private void UpdatePageNumber()
    {
        float pageNumber = Mathf.Ceil((puzzlePagesContainer.localPosition.x + 900) / pageWidth) + 1;
        string pageNumberTwoDigit = pageNumber < 10 ? "0" : "";
        string pageCountTwoDigit = pageObjects.Count < 10 ? "0" : "";
        pagesCount.text = $"{pageNumberTwoDigit + pageNumber}/{pageCountTwoDigit + pageObjects.Count}";
    }
}