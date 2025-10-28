using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ComicPin
{
    public string pinName;
    public Sprite pinImage;
}
public class ComicQuestionPanel : ComicPanel
{
    public ComicPin truePin;
    public ComicPin selectedPin;
    public Image blueQuestionMarkOverlay;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public override IEnumerator Play()
    {
        if (selectedPin.pinName == truePin.pinName)
        {
            yield return base.Play();
        }
        else
        {
            TrialManager.instance.DecreaseHealth(1f);
            ComicManager.instance.SwitchToPuzzleMode();
        }
    }

    public override void StartUpAnimation()
    {
        blueQuestionMarkOverlay.DOFade(1f, 0f);
        Vector2 originalPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = ComicManager.instance.questionPanelsSpawnLocation.anchoredPosition;
        rectTransform.localRotation = Quaternion.Euler(0, 0, -20);
        rectTransform.DOLocalRotate(Vector3.zero, 0.2f);
        rectTransform.DOAnchorPos(originalPos, 0.2f);
    }

    protected override void OnAppear()
    {
        blueQuestionMarkOverlay.DOFade(0f, 0.2f);
    }
}
