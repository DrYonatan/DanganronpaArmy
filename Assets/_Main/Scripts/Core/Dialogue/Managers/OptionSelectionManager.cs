using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DIALOGUE
{
    public class OptionSelectionManager : MonoBehaviour
    {
        public int selectedIndex;
        public List<UIOption> uiOptions;
        public UIOption optionPrefab;
        public RectTransform optionSelectionMenu;
        public AudioClip clickSound;
        public AudioClip moveSelectionSound;
        public bool isActive;

        void SelectionMenuControl()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                uiOptions[selectedIndex].OnDeselect();
                selectedIndex = (selectedIndex - 1 + uiOptions.Count) % uiOptions.Count;
                uiOptions[selectedIndex].OnSelect(); 
                SoundManager.instance.PlaySoundEffect(moveSelectionSound);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                uiOptions[selectedIndex].OnDeselect();
                selectedIndex = (selectedIndex + 1) % uiOptions.Count;
                uiOptions[selectedIndex].OnSelect();;
               SoundManager.instance.PlaySoundEffect(moveSelectionSound);
            }
        }
        
        void Update()
        {
            if (isActive && !PlayerInputManager.instance.isPaused)
               SelectionMenuControl();
        }

        public void GenerateUIOptions<T>(List<Option<T>> options) where T : DialogueNode
        {
            for (int i = 0; i < options.Count; i++)
            {
                UIOption newOption = Instantiate(optionPrefab, optionSelectionMenu);
                newOption.optionLabel.text = options[i].text;
                newOption.rectTransform.anchoredPosition = new Vector2(650f + i * 50f, i * -95f);
                newOption.originalX = newOption.rectTransform.anchoredPosition.x;
                uiOptions.Add(newOption);
            }
            optionSelectionMenu.anchoredPosition = new Vector2(100, 210);
            optionSelectionMenu.DOAnchorPosX(0, 0.2f);
        }

        public void OpenMenu<T>(List<Option<T>> options) where T : DialogueNode
        {
            isActive = true;
            gameObject.SetActive(true);
            GenerateUIOptions(options);
        }

        public void CloseMenu()
        {
            isActive = false;
            DestroyUIOptions();
            optionSelectionMenu.DOAnchorPosX(100, 0.3f).OnComplete(() => gameObject.SetActive(false));
        }

        void DestroyUIOptions()
        {
            foreach (UIOption option in uiOptions)
            {
                option.OnExit();
            }
            uiOptions.Clear();
        }

        public void ClickSelectedOption()
        {
            SoundManager.instance.PlaySoundEffect(clickSound);
            uiOptions[selectedIndex].OnClick();
        }
        
    }
}