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

    [SerializeField] private Image faceImage;
    public Image faceWhiteOverlay;
    public float faceFlashDuration;

    // Offsets for aligning each character's face inside the cropped box
    public List<CharacterInfo> characterInfos = new List<CharacterInfo>();

    public void SetFace(string characterName)
    {
        CharacterInfo info = characterInfos.Find(c => c.characterName == characterName);

        if(info != null)
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
    }
}
