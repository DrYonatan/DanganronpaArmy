using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterFaceController : MonoBehaviour
{ 

    [System.Serializable]
    public class CharacterInfo
    {
        public string characterName;
        public Sprite sprite;
        public Vector2 offset;
    }

    public RectTransform discussionFaceContainer;
    [SerializeField] private Image faceImage;
    public Image faceWhiteOverlay;
    public float faceFlashDuration;

    [SerializeField] private Image debateFaceImage;
    public Image debateFaceYellowOverlay;

    // Offsets for aligning each character's face inside the cropped box
    public List<CharacterInfo> characterInfos = new List<CharacterInfo>();

    public void SetFace(string characterName)
    {
        CharacterInfo info = characterInfos.Find(c => c.characterName == characterName);

        if(info != null && info.sprite != faceImage.sprite)
        {
            SetDiscussionFace(info);
            SetDebateFace(info);
        }
    }

    void SetDiscussionFace(CharacterInfo info)
    {
        Sequence seq = DOTween.Sequence();

        // Ensure white overlay is enabled and fully transparent
        faceWhiteOverlay.color = new Color(1, 1, 1, 0);
        faceWhiteOverlay.gameObject.SetActive(true);

        // Fade white overlay in
        seq.Append(faceWhiteOverlay.DOFade(1f, faceFlashDuration));

        // After flash, swap sprite
        seq.AppendCallback(() => {
            faceImage.sprite = info.sprite;
            faceImage.rectTransform.anchoredPosition = info.offset;
        });

        // Fade white overlay out
        seq.Append(faceWhiteOverlay.DOFade(0f, faceFlashDuration));
    }

    void SetDebateFace(CharacterInfo info)
    {
        Sequence seq = DOTween.Sequence();

        // Ensure white overlay is enabled and fully transparent
        debateFaceYellowOverlay.gameObject.SetActive(true);
        RectTransform faceRect = debateFaceYellowOverlay.GetComponent<RectTransform>();

        seq.Append(faceRect.DOScaleY(1f, faceFlashDuration));

        seq.AppendCallback(() => {
            debateFaceImage.sprite = info.sprite;
            debateFaceImage.rectTransform.anchoredPosition = info.offset;
        });

        seq.Append(faceRect.DOScaleY(0f, faceFlashDuration));
    }

    public void DiscussionFaceContainerAppear(float duration)
    {
        faceWhiteOverlay.color = Color.white;
        discussionFaceContainer.localScale = new Vector2(1f, 0f);
        discussionFaceContainer.transform.DOScaleY(1f, duration);
    }

    public void DiscussionFaceContainerDisappear(float duration)
    {
        discussionFaceContainer.localScale = new Vector2(1f, 1f);
        discussionFaceContainer.DOScaleY(0f, duration);
    }
}
