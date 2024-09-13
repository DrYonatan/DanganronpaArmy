using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class WorldManager : MonoBehaviour
{
    public RectTransform characterPanel = null;
    public Scene currentScene;

    public static WorldManager instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StartConversation("Prologue1");
        ProgressManager.instance.DecideWhichSceneToPlay();
    }

    void StartConversation(string textFile)
    {
        List<string> lines = FileManager.ReadTextAsset($"Scenes/Story/{textFile}");

        DialogueSystem.instance.Say(lines);

    }

    public void ReturningToWorld()
    {
        GameObject characters = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/1.5 - World Objects/Scene/Characters");
        GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Overlay/4 - Dialogue");

        if (currentScene.isFinished)
        {
            Destroy(characters);
            characters = null;
            List<string> lines = FileManager.ReadTextAsset($"Scenes/{currentScene.name}/Finish");
            if(lines != null)
            DialogueSystem.instance.Say(lines);
            ProgressManager.instance.DecideWhichSceneToPlay();
        }
        
        else
        dialogueBox.SetActive(false);


        if(currentScene != null)
        {
            GameObject prefab = Resources.Load<GameObject>($"Scenes/{currentScene.name}/Characters");
            if (characters == null)
            {

                Debug.Log("Working well");
                GameObject ob = Instantiate(prefab, characterPanel);
                ob.name = "Characters";
                ob.SetActive(true);


            }



            else
            {
                characters.transform.localScale = new Vector3(1, 1, 1);
            }
        }
       
        
        CameraManager.instance.ZoomCamera("out");

    }

    public void HideCharacters()
    {
        GameObject characters = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/1.5 - World Objects/Scene/Characters");

        Destroy(characters);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
