using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterFaceController : MonoBehaviour
{ 
    public RectTransform discussionFaceContainer;
    [SerializeField] private Image faceImage;
    public Image faceWhiteOverlay;
    public float faceFlashDuration;

    [SerializeField] private Image debateFaceImage;
    public Image debateFaceYellowOverlay;

    // Offsets for aligning each character's face inside the cropped box
    public void SetFace(Sprite sprite)
    {

        if(sprite != faceImage.sprite)
        {
            SetDiscussionFace(sprite);
            SetDebateFace(sprite);
        }
    }

    void SetDiscussionFace(Sprite sprite)
    {
        Sequence seq = DOTween.Sequence();

        // Ensure white overlay is enabled and fully transparent
        faceWhiteOverlay.color = new Color(1, 1, 1, 0);
        faceWhiteOverlay.gameObject.SetActive(true);

        // Fade white overlay in
        seq.Append(faceWhiteOverlay.DOFade(1f, faceFlashDuration));

        // After flash, swap sprite
        seq.AppendCallback(() => {
            faceImage.sprite = sprite;
            faceImage.color = sprite == null ? new Color(255, 255, 255, 0) : Color.white;
        });

        // Fade white overlay out
        seq.Append(faceWhiteOverlay.DOFade(0f, faceFlashDuration));
    }

    void SetDebateFace(Sprite sprite)
    {
        Sequence seq = DOTween.Sequence().SetUpdate(true);

        // Ensure white overlay is enabled and fully transparent
        debateFaceYellowOverlay.gameObject.SetActive(true);
        RectTransform faceRect = debateFaceYellowOverlay.GetComponent<RectTransform>();

        seq.Append(faceRect.DOScaleY(1f, faceFlashDuration));

        seq.AppendCallback(() => {
            debateFaceImage.sprite = sprite;
            debateFaceImage.color = sprite == null ? new Color(255, 255, 255, 0) : Color.white;
        });

        seq.Append(faceRect.DOScaleY(0f, faceFlashDuration));
    }

    public void DiscussionFaceContainerAppear(float duration)
    {
        faceWhiteOverlay.color = Color.white;
        discussionFaceContainer.localScale = new Vector3(1f, 0f, 1f);
        discussionFaceContainer.transform.DOScaleY(1f, duration);
    }

    public void DiscussionFaceContainerDisappear(float duration)
    {
        discussionFaceContainer.localScale = new Vector3(1f, 1f, 1f);
        discussionFaceContainer.DOScaleY(0f, duration);
    }
}
