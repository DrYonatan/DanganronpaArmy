using System.Collections;
using UnityEngine;
using System;
using COMMANDS;
using CHARACTERS;
using UnityEngine.UI;
using DIALOGUE;
using TMPro;
using System.Collections.Generic;


namespace TESTING
{
    public class CMD_DatabaseExtension_Examples : CMD_DatabaseExtension
    {

        new public static void Extend(CommandDatabase database)
        {
            //Add coroutine with no parameters
            // database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
            // database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
            // database.AddCommand("process_mp", new Func<string[], IEnumerator>(MultiLineProcess));

            database.AddCommand("CreateCharacter", new Action<string[]>(CreateCharacter));
            database.AddCommand("SwitchEmotion", new Action<string[]>(SwitchEmotion));
            database.AddCommand("MoveCamera", new Action<string>(MoveCamera));
            database.AddCommand("PlayUltimateVideo", new Action<string>(PlayUltimateVideo));
            database.AddCommand("HideCharacter", new Action<string>(HideCharacter));
            database.AddCommand("ReturnToWorld", new Action(ReturnToWorld));
            database.AddCommand("ShowCharacter", new Action<string>(ShowCharacter));
            database.AddCommand("DestroyCharacter", new Action<string>(DestroyCharacter));
            database.AddCommand("ShowImage", new Action<string>(ShowImage));
            database.AddCommand("HideImage", new Action(HideImage));
            database.AddCommand("ChangeBackground", new Action<string>(ChangeBackground));
            database.AddCommand("PlaySong", new Action<string>(PlaySong));
            database.AddCommand("StopSong", new Action(StopSong));
            database.AddCommand("ZoomCamera", new Action<string>(ZoomCamera));
            database.AddCommand("PlaySound", new Action<string>(PlaySound));
            database.AddCommand("PlayCutscene", new Action<string>(PlayCustscene));
            database.AddCommand("PlayCutsceneWithoutHiding", new Action<string>(PlayCutsceneWithoutHiding));
            database.AddCommand("PlayScene", new Action<string>(PlayScene));
            database.AddCommand("SetUltimateVideo", new Action<string>(SetUltimateVideo));
            database.AddCommand("SwitchTextColor", new Action<string>(SwitchTextColor));
            database.AddCommand("ShowMovingImage", new Action<string[]>(ShowMovingImage)); //for now the other variables are meaningless, it was meant to be for the size of the image.
            database.AddCommand("HideMovingImage", new Action(HideMovingImage));
            database.AddCommand("HideCutscene", new Action(HideCutscene));
            database.AddCommand("HideSceneCharacters", new Action(HideSceneCharacters));
            database.AddCommand("CreateCharacters", new Action<string[]>(CreateCharacters));
            database.AddCommand("HideBackground", new Action(HideBackground));
            database.AddCommand("LoadRoom", new Action<string>(LoadRoom));
            database.AddCommand("HideOverworldCharacters", new Action(HideOverworldCharacters));

        }

        private static void LoadRoom(string roomname)
        {
            Room room = Resources.Load<Room>($"Rooms/{roomname}");
            WorldManager.instance.StartLoadingRoom(room);
        }

        private static void HideSceneCharacters()
        {
            WorldManager.instance.HideCharacters();
        }

        private static void HideCutscene()
        {
            CutSceneManager.instance.Hide();
        }
        private static void PlayCutsceneWithoutHiding(string cutsceneName)
        {
            CutSceneManager.instance.MakeActive();
            CutSceneManager.instance.PlayCutsceneWithoutHiding(cutsceneName);
        }

        private static void HideMovingImage()
        {
            GameObject obj = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/3 - Cinematic/MovingImage");
            obj.SetActive(false);
        }
        private static void ShowMovingImage(string[] args)
        {
            GameObject obj = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/3 - Cinematic/MovingImage");
            obj.SetActive(true);
            Image movingImage = obj.GetComponent<Image>();
            movingImage.sprite = Resources.Load<Sprite>($"Images/{args[0]}");
        }
        private static void SwitchTextColor(string colorName)
        {
            GameObject text = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue/Root Container/DialogueText");
            if (colorName.ToLower() == "blue")
                text.GetComponent<TextMeshProUGUI>().color = Color.cyan;
            else if (colorName.ToLower() == "white")
                text.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        private static void PlayScene(string sceneName)
        {
            List<string> lines;
            lines = FileManager.ReadTextAsset($"GameEvents/{sceneName}");
            DialogueSystem.instance.Say(lines);
        }
        private static void PlayCustscene(string cutsceneName)
        {
            CutSceneManager.instance.MakeActive();
            CutSceneManager.instance.PlayCutscene(cutsceneName);
        }

        private static void PlaySound(string soundEffectName)
        {
            SoundManager.instance.PlaySoundEffect(soundEffectName);
        }
        private static void ZoomCamera(string zoom)
        {
            CameraManager.instance.ZoomCamera(zoom);
        }

        private static void StopSong()
        {
            MusicManager.instance.StopSong();
        }

        private static void PlaySong(string songName)
        {
            MusicManager.instance.PlaySong(songName);
        }

        private static void DestroyCharacter(string characterName)
        {
            CharacterManager.instance.DestroyCharacter(characterName);
        }

        private static void ChangeBackground(string imageName)
        {
            GameObject background = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/1 - Background/RawImage");
            background.GetComponent<CanvasGroup>().alpha = 1;
            background.GetComponent<RawImage>().texture = Resources.Load<Texture>($"Images/{imageName}");
        }

        private static void HideBackground() 
        {
            GameObject background = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/1 - Background/RawImage");
            background.GetComponent<CanvasGroup>().alpha = 0;
        }

        private static void ShowImage(string imageName)
        {
            ImageScript.instance.Show(imageName, 0.25f);
        }

        private static void HideImage()
        {
            ImageScript.instance.Hide(0.25f);
        }

        private static void ReturnToWorld()
        {
            DialogueSystem.instance.SetIsActive(false);
            CharacterManager.instance.DestroyAllCharacters();
            WorldManager.instance.currentGameEvent.UpdateEvent();
            GameObject characters = GameObject.Find("World/World Objects/Characters");
            if(characters != null)
            CharacterClickEffects.instance.MakeCharactersReappear(characters);
            WorldManager.instance.ReturningToWorld();
            DialogueSystem.instance.ClearTextBox();
            if(WorldManager.instance.currentRoom is PointAndClickRoom && !WorldManager.instance.currentGameEvent.isFinished)
            CameraManager.instance.ReturnToDollyTrack();
        }

        

        private static void ShowCharacter(string characterName)
        {
            CharacterManager.instance.ShowCharacter(characterName);
        }

        private static void HideCharacter(string characterName)
        {
            CharacterManager.instance.HideCharacter(characterName);
        }

        private static void PlayUltimateVideo(string characterName)
        {
            string[] args = {characterName, "default"};
            SwitchEmotion(args);
            VideoManager.instance.MakeActive();
            VideoManager.instance.PlayUltimateVideo(characterName);
            

        }

        private static void SetUltimateVideo(string characterName)
        {
            VideoManager.instance.SetUltimateVideo(characterName);
        }

        private static void CreateCharacter(string[] args)
        {
            Character character = CharacterManager.instance.CreateCharacter(args[0]);
            CharacterManager.instance.SetPosition(character.name, args[1]);
        }

        private static void CreateCharacters(string[] args)
        {
            string[] characterToCreate = new string[2];
            for(int i = 0; i+1 < args.Length; i+=2)
            {
                characterToCreate[0] = args[i];
                characterToCreate[1] = args[i+1];
                CreateCharacter(characterToCreate);
            }
        }

        private static void SwitchEmotion(string[] command)
        {

            Debug.Log($"Character Name: {command[0]}, Emotion: {command[1]}");

            CharacterManager.instance.SwitchEmotion(command[0].ToLower(), command[1]);

        }

        public static void MoveCamera(string direction)
        {
            CameraManager.instance.MoveCamera(direction, 0.3f);

        }

        private static void HideOverworldCharacters()
        {
            GameObject characters = GameObject.Find("World/World Objects/Characters");
            CharacterClickEffects.instance.MakeCharactersDisappear(characters);
        }

        private static IEnumerator SimpleProcess()
        {
            for (int i = 1; i <= 5; i++)
            {
                Debug.Log($"Process Running... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }

        private static IEnumerator LineProcess(string data)
        {
            if (int.TryParse(data, out int num))
            {
                for (int i = 0; i <= num; i++)
                {
                    Debug.Log($"Process Running... [{i}]");
                    yield return new WaitForSeconds(1);
                }
            }

        }

        private static IEnumerator MultiLineProcess(string[] data)
        {
            foreach (string line in data)
            {
                Debug.Log($"Process Message: '{line}'");
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}