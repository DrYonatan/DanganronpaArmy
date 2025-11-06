using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComicUIAnimator : MonoBehaviour
{
    public Image sideBars;
    
    public RectTransform questionPanelsSpawnLocation;

    public CanvasGroup puzzleCanvasGroup;
    public CanvasGroup solutionCanvasGroup;

    public RectTransform solutionPagesContainer;

    public RectTransform puzzlePagesContainer;
    public GridLayoutGroup pinsContainer;

    public RectTransform timerRect;
    public RectTransform pagesRect;

    public TextMeshProUGUI pagesCount;

    public ComicDraggablePin pinPrefab;

    public List<ComicPage> pageObjects = new List<ComicPage>();
    public List<ComicDraggablePin> draggablePins = new List<ComicDraggablePin>();

    private Vector2 pinsContainerOriginalPos;

    public ComicDraggablePin currentDraggedPin;

    public Image movingMist;
    public RectTransform frontMist;

    public NowIUnderstandAnimator nowIUnderstand;

    private Tween beatTween;

    public Image reEnactIcon;
    
    public float pagesContainerStartPos = -900;
    public float pageWidth = 440;

    public int pageNumber;
    public int firstPinNumber;
    
    public AudioClip pinsScrollSound;

    public AudioClip pagesScrollSound;

    public AudioClip readyToPresentSound;

    public ClimaxIntroAnimation introAnimation;

    private float timerOriginalX;
    private float pagesOriginalX;

    public IEnumerator Intro()
    {
        InitializeUI();
        AnimatePuzzleBackground();
        introAnimation.gameObject.SetActive(true);
        yield return introAnimation.PlayAnimation();
        introAnimation.gameObject.SetActive(false);
    }

    private void InitializeUI()
    {
        timerOriginalX = timerRect.anchoredPosition.x;
        pagesOriginalX = pagesRect.anchoredPosition.x;
        
        timerRect.DOAnchorPosX(timerOriginalX + 500f, 0f);
        pagesRect.DOAnchorPosX(pagesOriginalX - 700f, 0f);

        sideBars.DOFade(0f, 0f);
    }
    
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
        parent.AddComponent<CanvasGroup>();
        parent.transform.SetParent(pinsContainer.transform);
        parent.transform.localScale = Vector3.one;
        parent.transform.localPosition = Vector3.zero;
        parent.transform.localRotation = Quaternion.Euler(Vector3.zero);
        return parent;
    }

    public void ShowPuzzleUI()
    {
        puzzleCanvasGroup.alpha = 1f;
        solutionCanvasGroup.alpha = 0f;

        timerRect.DOAnchorPosX(timerOriginalX, 0.4f);
        pagesRect.DOAnchorPosX(pagesOriginalX, 0.4f);
        
        UpdatePageNumber();
        
        sideBars.DOFade(0.5f, 0f);
    }

    public void ShowSolutionUI()
    {
        puzzleCanvasGroup.alpha = 0f;
        solutionCanvasGroup.alpha = 1f;
    }

    public void LowerPinsContainer()
    {
        pinsContainer.GetComponent<RectTransform>().DOAnchorPosY(pinsContainerOriginalPos.y - 400, 0.2f);
    }

    public void RaisePinsContainer()
    {
        pinsContainer.GetComponent<RectTransform>().DOAnchorPosY(pinsContainerOriginalPos.y, 0.2f);
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

    public void JumpToNextPage()
    {
        SoundManager.instance.PlaySoundEffect(pagesScrollSound);
        pageNumber++;
        puzzlePagesContainer.localPosition = new Vector3(-900 + pageNumber * pageWidth, 0, 0);
        UpdatePageNumberText();
    }
    
    public void JumpToPrevPage()
    {
        SoundManager.instance.PlaySoundEffect(pagesScrollSound);
        pageNumber--;
        puzzlePagesContainer.localPosition = new Vector3(-900 + pageNumber * pageWidth, 0, 0);
        UpdatePageNumberText();
    }
    
    private void UpdatePageNumber()
    {
        pageNumber = (int)Mathf.Ceil((puzzlePagesContainer.localPosition.x + 900) / pageWidth);
        UpdatePageNumberText();
    }

    private void UpdatePageNumberText()
    {
        string pageNumberTwoDigit = pageNumber < 9 ? "0" : "";
        string pageCountTwoDigit = pageObjects.Count < 10 ? "0" : "";
        pagesCount.text = $"{pageNumberTwoDigit + (pageNumber+1)}/{pageCountTwoDigit + pageObjects.Count}";
    }

    public void ScrollPinContainer()
    {
        SoundManager.instance.PlaySoundEffect(pinsScrollSound);
        RectTransform pinsContainerTransform = pinsContainer.GetComponent<RectTransform>();
        float newX = pinsContainerOriginalPos.x + firstPinNumber * (pinsContainer.cellSize.x + pinsContainer.spacing.x);
        pinsContainerTransform.DOAnchorPosX(newX, 0.2f);
        
        UpdatePinsVisibility(0.3f);
    }

    public void UpdatePinsVisibility(float duration)
    {
        RectTransform pinsContainerTransform = pinsContainer.GetComponent<RectTransform>();
        int i = 0;
        foreach (RectTransform child in pinsContainerTransform)
        {
            UpdatePinVisibility(child.GetComponent<CanvasGroup>(), i, duration);
            i++;
        }
    }

    private void UpdatePinVisibility(CanvasGroup pin, int index, float duration)
    {
        if (index < firstPinNumber || index > firstPinNumber + 4)
        {
            pin.DOFade(0f, duration);
        }
        else
        {
            pin.DOFade(1f, duration);
        }
    }

    public void SetPinsContainerStartPos()
    {
        pinsContainerOriginalPos = pinsContainer.transform.localPosition;
    }

    public void BlinkReEnact()
    {
        SoundManager.instance.PlaySoundEffect(readyToPresentSound);
        reEnactIcon.DOFade(1f, 0.2f).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopBlinkingReEnact()
    {
        reEnactIcon.DOKill();
        reEnactIcon.DOFade(0f, 0.1f);
    }
}