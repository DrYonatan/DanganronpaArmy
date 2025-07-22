using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CourtTextBoxAnimator : TextBoxAnimations
{
    public RectTransform characterFace; // only appears in court

    public override void TextBoxAppear()
    {
        base.TextBoxAppear();

        characterFace.localScale = new Vector3(1f, 0f, 1f);
        characterFace.DOScaleY(1f, duration);
    }

    public override void TextBoxDisappear()
    {
        base.TextBoxDisappear();

        characterFace.localScale = new Vector3(1f, 1f, 1f);
        characterFace.DOScaleY(0f, duration);
                
    }

}
