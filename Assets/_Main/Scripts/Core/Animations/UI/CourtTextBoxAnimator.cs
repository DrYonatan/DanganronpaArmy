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

    public void ChangeFace(Sprite sprite)
    {
        characterFace.SetFace(sprite);
    }

    public override void Initialize()
    {
        base.Initialize();
        characterFace.discussionFaceContainer.DOScaleY(0f, 0f);
    }

    public CharacterCutIn InstantiateCutIn(GameObject cutIn)
    {
        CharacterCutIn characterCutIn = Instantiate(cutIn, transform).GetComponent<CharacterCutIn>();
        return characterCutIn;
    }
    
}