using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CourtTextBoxAnimator : TextBoxAnimations
{
    public CharacterFaceController characterFace;   

    public override void TextBoxAppear()
    {
        base.TextBoxAppear();

        characterFace.transform.localScale = new Vector3(1f, 0f, 1f);
        characterFace.transform.DOScaleY(1f, duration);
    }

    public override void TextBoxDisappear()
    {
        base.TextBoxDisappear();

        characterFace.transform.localScale = new Vector3(1f, 1f, 1f);
        characterFace.transform.DOScaleY(0f, duration);
                
    }

    public void ChangeFace(string characterName)
    {
        characterFace.SetFace(characterName);
    }


}
