using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName ="Game Events/Point And Click Event")]
public class PointAndClickEvent : GameEvent 
{
    public GameObject characterPrefab;
    public static string characterPath = $"World Objects/Characters";
    public TextAsset finishText;

    public override void UpdateEvent()
    {
        isFinished = true;
        Transform characters = GameObject.Find("World").transform.GetChild(0).transform.Find(characterPath).transform;

        foreach (Transform character in characters)
        {
            if (!character.GetComponent<WorldObjectsInteraction>().isClicked)
                isFinished = false;
        }
    }

    public override void CheckIfFinished()
    {
        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue");
        CameraManager.instance.MoveCameraTo(GameObject.Find("World").transform.GetChild(0).transform.Find("CameraStartPos"));
        if(isFinished)
        OnFinish();
        else
        dialogueBox.SetActive(false);
    }


    public override void PlayEvent()
    {
        Transform characters = GameObject.Find("World").transform.GetChild(0).transform.Find(characterPath);
        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue");
        ((PointAndClickRoom)(WorldManager.instance.currentRoom)).ResetRotations();

        if (characters == null)
        {
            WorldManager.instance.CreateCharacters(characterPrefab);
        }
        else
        {
            characters.gameObject.SetActive(true);
        }
        
    }

    public override void OnFinish()
    {
        GameObject characters = GameObject.Find("World").transform.GetChild(0).transform.Find(characterPath).gameObject;
        Destroy(characters);
        List<string> lines = FileManager.ReadTextAsset(finishText);
        if (lines != null)
            DialogueSystem.instance.Say(lines);
        ProgressManager.instance.DecideWhichSceneToPlay();

        
    }

}
