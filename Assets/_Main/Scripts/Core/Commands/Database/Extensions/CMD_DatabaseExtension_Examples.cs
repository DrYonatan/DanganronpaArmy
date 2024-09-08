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
            //Add Action with no parameters
            database.AddCommand("print", new Action(PrintDefaultMessage));
            database.AddCommand("print_1p", new Action<string>(PrintUserMessage));
            database.AddCommand("print_mp", new Action<string[]>(PrintLines));

            //Add lambda with no parameters
            database.AddCommand("lambda", new Action(() => { Debug.Log("Printing a default message to console from lambda command"); }));
            database.AddCommand("lambda_1p", new Action<string>((arg) => { Debug.Log($"Log user lambda message: '{arg}'"); }));
            database.AddCommand("lambda_mp", new Action<string[]>((args) => { Debug.Log(string.Join(", ", args)); }));

            //Add coroutine with no parameters
            database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
            database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
            database.AddCommand("process_mp", new Func<string[], IEnumerator>(MultiLineProcess));

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
            GameObject obj = GameObject.Find("VN controller/Root/Canvas - Overlay/MovingImage");
            obj.SetActive(false);
        }
        private static void ShowMovingImage(string[] args)
        {
            /* GameObject imgObject = new GameObject("movingImage");

             RectTransform trans = imgObject.AddComponent<RectTransform>();
             trans.anchoredPosition = new Vector2(0.5f, 0.5f);
             trans.localPosition = new Vector3(0, 0, 0);
             trans.position = new Vector3(983, 540, 0);

             Image image = imgObject.AddComponent<Image>();
             image.sprite = Resources.Load<Sprite>($"Images/{args[0]}");
             image.rectTransform.sizeDelta = new Vector2(2386, 1080);
             imgObject.transform.SetParent(GameObject.Find("VN controller/Root/Canvas - Overlay/movingImage").transform);
            */
            GameObject obj = GameObject.Find("VN controller/Root/Canvas - Overlay/MovingImage");
            obj.SetActive(true);
            Image movingImage = obj.GetComponent<Image>();
            movingImage.sprite = Resources.Load<Sprite>($"Images/{args[0]}");
            

            
        }
        private static void SwitchTextColor(string colorName)
        {
            GameObject text = GameObject.Find("VN controller/Root/Canvas - Overlay/4 - Dialogue/Root Container/DialogueText");
            if (colorName.ToLower() == "blue")
                text.GetComponent<TextMeshProUGUI>().color = Color.cyan;
            else if (colorName.ToLower() == "white")
                text.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        private static void PlayScene(string sceneName)
        {
            List<string> lines;
            lines = FileManager.ReadTextAsset($"Scenes/{sceneName}");
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
            Debug.Log(background == null);
            background.GetComponent<RawImage>().texture = Resources.Load<Texture>($"Images/{imageName}");
        }

        private static void ShowImage(string imageName)
        {
            GameObject image = GameObject.Find("VN controller/Root/Canvas - Overlay/Image");
            image.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{imageName}");
            ImageScript.instance.Show();


        }

        private static void HideImage()
        {
            ImageScript.instance.Hide();
        }

        private static void ReturnToWorld()
        {
            WorldManager.instance.ReturningToWorld();
            
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
            
            character.Show();
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
        private static void PrintDefaultMessage()
        {
            Debug.Log("Printing a default message to console");
        }

        private static void PrintUserMessage(string message)
        {
            Debug.Log($"User's message: '{message}'");
        }

        private static void PrintLines(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"{i++}. '{line}'");
            }
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