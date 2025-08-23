using DG.Tweening;
using UnityEngine;

public class CourtTextBoxAnimator : TextBoxAnimations
{
    public CharacterFaceController characterFace;

    public void FaceAppear()
    {
        characterFace.DiscussionFaceContainerAppear(duration);
    }

    public override void TextBoxDisappear()
    {
        base.TextBoxDisappear();

        characterFace.DiscussionFaceContainerDisappear(duration);
    }

    public void ChangeFace(string characterName)
    {
        characterFace.SetFace(characterName);
    }
}