using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName ="DialogueSystem/Scene")]
public class Scene : GameEvent 
{
   
    public bool isFinished = false;
    public List<string> characters;
    public List<Scene> conditionScenes;
    public GameObject characterPrefab;
   

    public Scene(string name, List<string> characters)
    {
        
        this.characters = characters;
    }

    public override void UpdateEvent()
    {
        isFinished = true;

        foreach (string character in characters)
        {
            GameObject obj = GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/1.5 - World Objects/Scene/Characters/Character - [{character}] Variant");
            if (!obj.GetComponent<WorldObjectsInteraction>().isClicked)
                isFinished = false;
        }
    }

    public override bool CheckIfFinished()
    {
        return isFinished;
    }

    public override bool CheckIfToPlay()
    {
        bool playScene = true;
        if (conditionScenes != null)
            foreach (Scene conditionScene in conditionScenes)
            {
                if (!conditionScene.CheckIfFinished())
                    playScene = false;

            }

        
        return playScene;
    }

    public override void PlayEvent()
    {
        GameObject characters = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/1.5 - World Objects/Scene/Characters");
        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Overlay/4 - Dialogue");

        GameObject prefab = Resources.Load<GameObject>($"Scenes/{name}/Characters");
        if (characters == null)
        {
            WorldManager.instance.CreateCharacters(prefab);
        }
        else
        {
            characters.transform.localScale = new Vector3(1, 1, 1);
        }
        CameraManager.instance.ZoomCamera("out");

    }

    public override void OnFinish()
    {
        GameObject characters = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/1.5 - World Objects/Scene/Characters");

        Destroy(characters);
        List<string> lines = FileManager.ReadTextAsset($"Scenes/{name}/Finish");
        if (lines != null)
            DialogueSystem.instance.Say(lines);
        ProgressManager.instance.DecideWhichSceneToPlay();

        
    }
}
