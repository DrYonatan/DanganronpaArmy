using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName ="DialogueSystem/Scene")]
public class Scene : GameEvent 
{
   
    public List<string> characters;
    public List<Scene> conditionScenes;
    public GameObject characterPrefab;
    public static string characterPath = $"World/World Objects/Characters";
   

    public Scene(string name, List<string> characters)
    {
        this.characters = characters;
    }

    public override void UpdateEvent()
    {
        isFinished = true;

        foreach (string character in characters)
        {
            Debug.Log("character name:" + character);
            GameObject obj = GameObject.Find($"{characterPath}/{character}");
            if (!obj.GetComponent<WorldObjectsInteraction>().isClicked)
                isFinished = false;
        }
    }

    public override void CheckIfFinished()
    {
        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue");
        CameraManager.instance.MoveCameraTo(GameObject.Find("World/CameraStartPos").transform);
        if(isFinished)
        OnFinish();
        else
        dialogueBox.SetActive(false);
    }

    public override bool CheckIfToPlay()
    {
        bool playScene = true;
        if (conditionScenes != null)
            foreach (Scene conditionScene in conditionScenes)
            {
                if (!conditionScene.isFinished)
                    playScene = false;

            }

        
        return playScene;
    }

    public override void PlayEvent()
    {
        GameObject characters = GameObject.Find(characterPath);
        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue");

        GameObject prefab = Resources.Load<GameObject>($"Scenes/{name}/Characters");
        if (characters == null)
        {
            WorldManager.instance.CreateCharacters(prefab);
        }
        else
        {
            characters.SetActive(true);
        }
        
    }

    public override void OnFinish()
    {
        GameObject characters = GameObject.Find(characterPath);
        Destroy(characters);
        List<string> lines = FileManager.ReadTextAsset($"Scenes/{name}/Finish");
        if (lines != null)
            DialogueSystem.instance.Say(lines);
        ProgressManager.instance.DecideWhichSceneToPlay();

        
    }
}
