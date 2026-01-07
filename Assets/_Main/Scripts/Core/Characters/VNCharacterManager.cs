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
        
        Dictionary<Character, GameObject> characterObjects = new ();
        private void Awake()
        {
            instance = this;
        }
        public void CreateCharacter(CharacterPositionMapping characterInfo)
        {
            GameObject characterObj = Instantiate(characterInfo.character.vnObjectPrefab, characterLayer);
            characterObj.transform.localPosition = Vector3.zero;
            CanvasGroup canvasGroup = characterObj.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, 0.25f);
            characterObj.name = characterInfo.character.name;
            characterObj.transform.localPosition = new Vector3(GetCharacterPosition((CameraLookDirection)characterInfo.position).x, characterInfo.character.vnObjectPrefab.transform.localPosition.y, 0);
            characterObjects.Add(characterInfo.character, characterObj);
        }

        public void ShowOnlySpeaker(Character character, float duration)
        {
            foreach (KeyValuePair<Character, GameObject> characterObj in characterObjects)
            {
                if (characterObj.Key.Equals(character))
                {
                    ShowCharacter(characterObj.Value, duration);
                }
                else
                {
                    HideCharacter(characterObj.Value, duration);
                }
            }
        }

        public void HideAllCharacters()
        {
            foreach (KeyValuePair<Character, GameObject> characterObj in characterObjects)
            {
                HideCharacter(characterObj.Value, 0);
            }
        }

        void ShowCharacter(GameObject characterObj, float duration)
        {
            CanvasGroup canvasGroup = characterObj.GetComponent<CanvasGroup>();
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f,  duration);
        }
        void HideCharacter(GameObject characterObj, float duration)
        {
            CanvasGroup canvasGroup = characterObj.GetComponent<CanvasGroup>();
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f,  duration);
        }

        public void DestroyCharacters()
        {
            foreach (KeyValuePair<Character, GameObject> characterObj in characterObjects)
            {
                Destroy(characterObj.Value);
            }
            characterObjects.Clear();
        }

        public void SwitchEmotion(Character character, CharacterState expression)
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
            Sprite sprite = expression.sprite;
            if (sprite.Equals(oldSprite.sprite))
                return;
            
            newSprite.sprite = sprite;
            // Fade out + destroy old
            oldSprite.DOKill();
            oldSprite.DOFade(0f, 0.25f).SetLink(oldSprite.gameObject).OnComplete(() =>
            {
                if (oldSpriteObj != null)
                    Destroy(oldSpriteObj);
            });

            // Fade in new
            newSprite.color = Color.black;
            newSprite.DOColor(Color.white, 0.25f).SetLink(newSprite.gameObject);
        }


        Vector3 GetCharacterPosition(CameraLookDirection lookDirection)
        {
            Vector3 res = new Vector3();
            switch (lookDirection)
            {
                case CameraLookDirection.Left:
                    res = left.localPosition;
                    break;
                case CameraLookDirection.MidLeft:
                    res = midLeft.localPosition;
                    break;
                case CameraLookDirection.Middle:
                    res = middle.localPosition;
                    break;
                case CameraLookDirection.MidRight:
                    res = midRight.localPosition;
                    break;
                case CameraLookDirection.Right:
                    res = right.localPosition;
                    break;
            }
            
            return res;
        }

    }
}