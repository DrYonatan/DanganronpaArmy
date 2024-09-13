using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance { get; private set; }
    public List<Scene> scenes = new List<Scene>();
    private void Awake()
    {
        instance = this;
        //List<string> characters1 = new List<string>();
        //characters1.Add("Koby");
        //characters1.Add("Inbal");
        //characters1.Add("Noya");
        //characters1.Add("Noa");
        //characters1.Add("Guy");

        //scenes.Add(new Scene("Scene1", characters1));

        //List<string> characters2 = new List<string>();
        //characters2.Add("Ariel");
        //characters2.Add("Kfir");
        //characters2.Add("Omer");
        //characters2.Add("Maya");

        //scenes.Add(new Scene("Scene2", characters2));

        //List<string> characters3 = new List<string>();
        //characters3.Add("Roey");
        //characters3.Add("Liel");
        //characters3.Add("Shiraz");
        //characters3.Add("Romi");
        //characters3.Add("Ohav");

        //scenes.Add(new Scene("Scene3", characters3));
    }

    public void DecideWhichSceneToPlay()
    {
        foreach(Scene scene in scenes)
        {
            bool playScene = true;
            if(!scene.isFinished)
            {
                if(scene.conditionScenes != null)
                foreach(Scene conditionScene in scene.conditionScenes)
                {
                    if (!conditionScene.isFinished)
                        playScene = false;

                }
                if(playScene)
                {
                    WorldManager.instance.currentScene = scene;
                }
            }
        }

        //if(!GetSceneByName("Scene1").isFinished)
        //WorldManager.instance.currentScene = GetSceneByName("Scene1");
        //if (GetSceneByName("Scene1").isFinished)
        //{
        //    Scene scene = GetSceneByName("Scene2");
        //    if(!scene.isFinished)
        //    WorldManager.instance.currentScene = scene;
        //}

        //if(GetSceneByName("Scene2").isFinished)
        //{
        //    Scene scene = GetSceneByName("Scene3");
        //    if (!scene.isFinished)
        //        WorldManager.instance.currentScene = scene;
        //}
        //if(GetSceneByName("Scene3").isFinished)
        //{
        //    WorldManager.instance.currentScene = null;
        //}
        
    }

    public Scene GetSceneByName(string name)
    {
        foreach(Scene scene in scenes)
        {
            if (scene.name == name)
                return scene;
        }
        return null;
    }
}
