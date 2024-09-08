using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace CHARACTERS
{
    [System.Serializable]
    public class CharacterConfigData
    {
        public string name;
        public string alias;
        public Character.CharacterType characterType;

        public Color nameColor;
        public Color dialogueColor;

        public CharacterConfigData Copy()
        {
            CharacterConfigData result = new CharacterConfigData();

            result.name = name;
            result.alias = alias;
            result.characterType = characterType;

            result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

            return result;
        }

        private static Color defaultColor => DialogueSystem.instance.config.defaultTextColor;
        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();

                result.name = "";
                result.alias = "";
                result.characterType = Character.CharacterType.Text;

                result.nameColor = defaultColor;
                result.dialogueColor = defaultColor;

                return result;
            }
        }
    }
}