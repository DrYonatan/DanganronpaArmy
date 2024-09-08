using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene
{
    public string name;
    public bool isFinished = false;
    public List<string> characters;

    public Scene(string name, List<string> characters)
    {
        this.name = name;
        this.characters = characters;
    }

    public void UpdateScene()
    {
        isFinished = true;

        foreach (string character in characters)
        {
            GameObject obj = GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/1.5 - World Objects/Scene/Characters/Character - [{character}] Variant");
            if (!obj.GetComponent<WorldObjectsInteraction>().isClicked)
                isFinished = false;
        }
    }
}
