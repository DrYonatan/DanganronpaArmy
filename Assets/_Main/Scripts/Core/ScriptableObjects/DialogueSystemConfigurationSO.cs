using CHARACTERS;
using UnityEngine;

namespace DIALOGUE
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "DialogueSystem/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public CharacterConfigSO characterConfigurationAsset;

        public Color defaultTextColor = Color.white;
    }
}