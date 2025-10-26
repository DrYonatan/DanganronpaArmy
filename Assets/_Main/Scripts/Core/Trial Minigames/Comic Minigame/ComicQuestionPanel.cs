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
    public override IEnumerator Play()
    {
        if (selectedPin.pinName == truePin.pinName)
        {
            yield return base.Play();
        }
        else
        {
            // TODO false pin logic
        }
    }

    public override void StartUpAnimation()
    {
        blueQuestionMarkOverlay.DOFade(1f, 0f);
    }

    protected override void OnAppear()
    {
        blueQuestionMarkOverlay.DOFade(0f, 0.2f);
    }
}
