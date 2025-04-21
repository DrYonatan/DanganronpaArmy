using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DIALOGUE
{


public class PlayerInputManager : MonoBehaviour
{
        
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        bool isPaused = WorldManager.instance.isPaused;
        if(!isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {  
               PromptAdvance();       
            }
        } 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TogglePause();
        }    
    }

    public void PromptAdvance()
    {
        if(!VideoManager.instance.isPlaying && !CutSceneManager.instance.isPlaying) 
        {
             DialogueSystem.instance.OnUserPrompt_Next();
        }
    }

    public void TogglePause()
    {
        WorldManager.instance.isPaused = !WorldManager.instance.isPaused;
        GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/6 - Controls/Pause Menu").SetActive(WorldManager.instance.isPaused);
        GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/6 - Controls/Reticle").SetActive(!WorldManager.instance.isPaused);
    }
}
}
