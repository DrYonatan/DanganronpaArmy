using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class VNCharacterManager : MonoBehaviour
    {
        public static VNCharacterManager instance { get; private set; }

        public RectTransform characterLayer;

        public RectTransform left;
        public RectTransform midLeft;
        public RectTransform middle;
        public RectTransform midRight;
        public RectTransform right;
        
        Dictionary<CharacterCourt, GameObject> characterObjects = new ();
        private void Awake()
        {
            instance = this;
        }
        public void CreateCharacter(VNCharacterInfo characterInfo)
        {
            GameObject characterObj = Instantiate(characterInfo.Character.vnObjectPrefab, characterLayer);
            characterObj.GetComponent<CanvasGroup>().alpha = 1f;
            characterObj.name = characterInfo.Character.name;
            characterObj.transform.position = new Vector3(GetCharacterPosition(characterInfo.LookDirection).x, characterObj.transform.position.y, characterObj.transform.position.z);
            characterObjects.Add(characterInfo.Character, characterObj);
        }

        public void ShowOnlySpeaker(VNCharacterInfo characterInfo)
        {
            foreach (KeyValuePair<CharacterCourt, GameObject> characterObj in characterObjects)
            {
                if (characterObj.Key.Equals(characterInfo.Character))
                {
                    ShowCharacter(characterObj.Value);
                }
                else
                {
                    HideCharacter(characterObj.Value);
                }
            }
        }

        void ShowCharacter(GameObject characterObj)
        {
            CanvasGroup canvasGroup = characterObj.GetComponent<CanvasGroup>();
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f,  0.25f);
        }
        void HideCharacter(GameObject characterObj)
        {
            CanvasGroup canvasGroup = characterObj.GetComponent<CanvasGroup>();
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f,  0.25f);
        }

        public void DestroyCharacters()
        {
            foreach (KeyValuePair<CharacterCourt, GameObject> characterObj in characterObjects)
            {
                Destroy(characterObj.Value);
            }
        }

        public void SwitchEmotion(CharacterCourt character, CharacterState expression)
        {
            Transform characterTransform = characterObjects[character].transform;

            // Clean up any extra sprites (just in case)
            for (int i = characterTransform.childCount - 1; i >= 1; i--)
            {
                characterTransform.GetChild(i).GetComponent<Image>().DOKill();
                Destroy(characterTransform.GetChild(i).gameObject);
            }

            GameObject oldSpriteObj = characterTransform.GetChild(0).gameObject;

            // Create the new sprite
            GameObject newSpriteObj = Instantiate(oldSpriteObj, characterTransform);
            newSpriteObj.transform.SetAsFirstSibling();
            newSpriteObj.name = oldSpriteObj.name;

            Image oldSprite = oldSpriteObj.GetComponent<Image>();
            Image newSprite = newSpriteObj.GetComponent<Image>();
            newSprite.sprite = character.Sprites[(int)expression];

            // Fade out + destroy old
            oldSprite.DOKill();
            oldSprite.DOFade(0f, 0.25f).OnComplete(() =>
            {
                if (oldSpriteObj != null)
                    Destroy(oldSpriteObj);
            });

            // Fade in new
            newSprite.color = Color.black;
            newSprite.DOColor(Color.white, 0.25f);
        }


        Vector3 GetCharacterPosition(CameraLookDirection lookDirection)
        {
            Vector3 res = new Vector3();
            switch (lookDirection)
            {
                case CameraLookDirection.Left:
                    res = left.position;
                    break;
                case CameraLookDirection.MidLeft:
                    res = midLeft.position;
                    break;
                case CameraLookDirection.Middle:
                    res = middle.position;
                    break;
                case CameraLookDirection.MidRight:
                    res = midRight.position;
                    break;
                case CameraLookDirection.Right:
                    res = right.position;
                    break;
            }
            
            return res;
        }

    }
}