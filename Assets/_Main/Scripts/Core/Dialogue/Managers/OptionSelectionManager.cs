using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class OptionSelectionManager : MonoBehaviour
    {
        public List<Option<DialogueNode>> options;
        public int selectedIndex;

        void SelectionMenuControl()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                selectedIndex--;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                selectedIndex++;
            }
        }
        
        void Update()
        {
            SelectionMenuControl();
        }
        
    }
}