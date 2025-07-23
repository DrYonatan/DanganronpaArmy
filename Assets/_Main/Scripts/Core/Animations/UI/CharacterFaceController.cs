using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Offsets for aligning each character's face inside the cropped box
    public List<CharacterInfo> characterInfos = new List<CharacterInfo>();

    public void SetFace(string characterName)
    {
        CharacterInfo info = characterInfos.Find(c => c.characterName == characterName);

        if(info != null)
        {
            faceImage.sprite = info.sprite;
            faceImage.rectTransform.anchoredPosition = info.offset;
        }

        else
        {
            Debug.Log($"Character {characterName}  does not exist!");
        }
    }
}
