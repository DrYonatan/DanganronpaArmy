using System;
using System.Collections;
using UnityEngine;

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
}
