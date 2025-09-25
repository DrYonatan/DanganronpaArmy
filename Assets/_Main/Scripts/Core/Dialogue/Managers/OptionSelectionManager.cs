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

        void SelectionMenuControl()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                uiOptions[selectedIndex].OnDeselect();
                selectedIndex = (selectedIndex - 1 + uiOptions.Count) % uiOptions.Count;
                uiOptions[selectedIndex].OnSelect();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                uiOptions[selectedIndex].OnDeselect();
                selectedIndex = (selectedIndex + 1) % uiOptions.Count;
                uiOptions[selectedIndex].OnSelect();;
            }
        }
        
        void Update()
        {
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

        public void DestroyUIOptions()
        {
            foreach (UIOption option in uiOptions)
            {
                Destroy(option.gameObject);
            }
            uiOptions.Clear();
        }
        
    }
}