using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Linq;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class CharacterManager : MonoBehaviour, ICharacterHandler
    { 
        public static CharacterManager instance { get; private set; }
        public Dictionary<string, Character> characters = new Dictionary<string, Character>();
        public List<string> leftCharacters;
        public List<string> middleCharacters;
        public List<string> rightCharacters;

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

        public void InstantHideCharacter(string characterName) 
        {
            GameObject character = GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [{characterName}]");
            if(character)
            character.GetComponent<CanvasGroup>().alpha = 0;
        }

   
        
        public void SetPosition(string characterName, string position)
        {
            GameObject characterParent = GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [{characterName}]/Anim/Renderers/Later: 0");
            GameObject middle = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Middle");
            GameObject right = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Right");
            GameObject left = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Left");
            Vector3 middleVec = new Vector3(middle.transform.position.x, characterParent.transform.position.y, middle.transform.position.z);
            Vector3 rightVec = new Vector3(right.transform.position.x, characterParent.transform.position.y, right.transform.position.z);
            Vector3 leftVec = new Vector3(left.transform.position.x, characterParent.transform.position.y, left.transform.position.z);



            switch (position)
            {
                case "middle":
                    characterParent.transform.position = middleVec;
                    middleCharacters.Add(characterName);
                    break;

                case "left":
                    characterParent.transform.position = leftVec;
                    leftCharacters.Add(characterName);
                    break;

                case "right":
                    characterParent.transform.position = rightVec;
                    rightCharacters.Add(characterName);
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

        public void DestroyAllCharacters()
        {
            leftCharacters = new List<string>();
            middleCharacters = new List<string>();
            rightCharacters = new List<string>();

            foreach(string character in characters.Keys) 
            {
                string tempstring = character[0].ToString();
                tempstring=tempstring.ToUpper();
                string capitalLetterCharacter = character;
                capitalLetterCharacter = capitalLetterCharacter.Remove(0, 1);
                capitalLetterCharacter = capitalLetterCharacter.Insert(0, tempstring);
                // this entire thing turns the first letter capital

                DestroyCharacter(capitalLetterCharacter);
            }

            characters = new Dictionary<string, Character>();
        }
        
        public string GetCharacterPosition(string speakerName)
        {
            string direction = "middle";
            if(leftCharacters.Contains(speakerName))
                direction = "left";
            else if(rightCharacters.Contains(speakerName))
                direction = "right";

            return direction;
        }
        public string GetSpeakerEnglishName(DIALOGUE_LINE line)
        {
            string speakerName = "";
            foreach (CharacterConfigData character in DialogueSystem.instance.config.characterConfigurationAsset.characters)
            {
                if (character.alias == line.speakerData.name)
                {
                    speakerName = character.name;
                    break;
                }
            }
            return speakerName;
        }
        
        public void DecideCharactersToHide(string speakerName)
        {
            string pos = GetCharacterPosition(speakerName);
            
            if(pos == "left") 
            {
                foreach(string character in leftCharacters) {
                    if(character != speakerName)
                        InstantHideCharacter(character);
                }
            }
            else if(pos == "middle") 
            {
                foreach(string character in middleCharacters) {
                    if(character != speakerName)
                        InstantHideCharacter(character);
                }
            }
            else 
            {
                foreach(string character in rightCharacters) {
                    if(character != speakerName)
                        InstantHideCharacter(character);
                }
            }

        }

        public void OnLineParsed(DIALOGUE_LINE line)
        {
            string speakerName = "";
            if (line.hasSpeaker)
            {
                speakerName = GetSpeakerEnglishName(line);
                string direction = GetCharacterPosition(speakerName);
                if (characters.ContainsKey(speakerName.ToLower()))
                    CameraManager.instance.MoveCamera(direction, 0.3f);
                if(!speakerName.Equals(""))
                    DecideCharactersToHide(speakerName);
            }

            ShowCharacter(speakerName);
        }

        public void OnStopConversation()
        {
            DestroyAllCharacters();
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