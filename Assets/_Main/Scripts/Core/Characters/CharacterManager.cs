using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Linq;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class CharacterManager : MonoBehaviour
    {
   
        
        public static CharacterManager instance { get; private set; }
        public Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfigurationAsset;

        private string characterSpritesPath => $"Characters/{CHARACTERNAME_ID}/Images";
        private const string CHARACTERNAME_ID = "<charname>";
        public string characterRootPathFormat => $"Characters/{CHARACTERNAME_ID}";
        public string characterPrefabNameFormat => $"Character - [{CHARACTERNAME_ID}]";
        public string characterPrefabPathFormat => $"{characterRootPathFormat}/{characterPrefabNameFormat}";

        [SerializeField] private RectTransform _characterpanel = null;
        public RectTransform characterPanel => _characterpanel;        
        private void Awake()
        {
            instance = this;
        }

        public Character CreateCharacter(string characterName)
        {
            if(characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A Character named '{characterName}' already exists");
                return null;
            }

            CHARACTER_INFO info = GetCharacterInfo(characterName);

            Character character = CreateCharacterFromInfo(info);

            characters.Add(characterName.ToLower(), character);



            return character;
        }

       
        public void DestroyCharacter(string characterName)
        {
            characters.Remove(characters.Single(s => s.Key == characterName.ToLower()).Key);
            GameObject character = GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [{characterName}]");
            Destroy(character);
        }


        public void ShowCharacter(string characterName)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                (characters.Single(s => s.Key == characterName.ToLower()).Value).Show();
            }
        }
        public void HideCharacter(string characterName)
        {
            
            if (characters.ContainsKey(characterName.ToLower()))
            {
                (characters.Single(s => s.Key == characterName.ToLower()).Value).Hide();
            }
        }

   
        
        public void SetPosition(string characterName, string position)
        {
            GameObject characterParent = GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [{characterName}]/Anim/Renderers/Later: 0");
            GameObject middle = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/0 - Underlay Background/Middle");
            GameObject right = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/0 - Underlay Background/Right");
            GameObject left = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/0 - Underlay Background/Left");
            Vector3 middleVec = new Vector3(middle.transform.position.x, characterParent.transform.position.y, characterParent.transform.position.z);
            Vector3 rightVec = new Vector3(right.transform.position.x, characterParent.transform.position.y, characterParent.transform.position.z);
            Vector3 leftVec = new Vector3(left.transform.position.x, characterParent.transform.position.y, characterParent.transform.position.z);



            switch (position)
            {
                case "middle":
                    characterParent.transform.position = middleVec;
                    break;

                case "left":
                    characterParent.transform.position = leftVec;
                    break;

                case "right":
                    characterParent.transform.position = rightVec;
                    break;
            }
        }

        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new CHARACTER_INFO();

            result.name = characterName;

            result.config = config.GetConfig(characterName);

            result.prefab = GetPrefabForCharacter(characterName);

            result.rootCharacterFolder = FormatCharacterPath(characterRootPathFormat, characterName);

      

            return result;
        }

       

      
        public void SwitchEmotion(string characterName, string spriteName)
        {

            Sprite sprite = (characters[characterName] as Character_Sprite).GetSprite(spriteName);
        
            (characters[characterName] as Character_Sprite).TransitionSprite(sprite);

//            string imagePath = "VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [" + characterName + "](Clone)/Anim/FullBody";
  //          GameObject obj = GameObject.Find(imagePath);
    //        obj.GetComponent<Image>().sprite = GetSpritesForCharacter(characterName)[num-1];
           
            

            // Debug.Log(imagePath);
            // Debug.Log(obj == null);
            //Debug.Log(GetSpritesForCharacter(characterName).Count);


        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(characterPrefabPathFormat, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        public string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTERNAME_ID, characterName);


        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            CharacterConfigData config = info.config;

            switch(info.config.characterType)
            {
                case Character.CharacterType.Text:
                  return new Character_Text(info.name, config);

                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                  return new Character_Sprite(info.name, config, info.prefab, info.rootCharacterFolder);
            }
          

            return null;
        }

        

        private class CHARACTER_INFO
        {
            public string name = "";

            public string rootCharacterFolder = "";

            public CharacterConfigData config = null;

            public GameObject prefab = null;

         
        }
    }
}