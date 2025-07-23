using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CourtTextBoxAnimator : TextBoxAnimations
{
    public CharacterFaceController characterFace;
    public Image faceWhiteOverlay;
    public float faceFlashDuration;

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
        Sequence seq = DOTween.Sequence();

        // Ensure white overlay is enabled and fully transparent
        faceWhiteOverlay.color = new Color(1, 1, 1, 0);
        faceWhiteOverlay.gameObject.SetActive(true);

        // Fade white overlay in
        seq.Append(faceWhiteOverlay.DOFade(1f, faceFlashDuration));

        // After flash, swap sprite
        seq.AppendCallback(() => {
            characterFace.SetFace(characterName);
        });

        // Fade white overlay out
        seq.Append(faceWhiteOverlay.DOFade(0f, faceFlashDuration));

        // Optional: hide overlay after
        seq.AppendCallback(() => {
            faceWhiteOverlay.gameObject.SetActive(false);
        });
        
    }


}
