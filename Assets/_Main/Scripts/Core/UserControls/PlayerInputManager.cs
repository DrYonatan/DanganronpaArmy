using UnityEngine;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance {get; private set;}

        public bool isPaused;

        void Start()
        {
            isPaused = false;
            instance = this;
        }
        // Update is called once per frame
        void Update()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            if (!isPaused)
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
            if (!VideoManager.instance.isPlaying && !CutSceneManager.instance.isPlaying)
            {
                DialogueSystem.instance.OnUserPrompt_Next();
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/6 - Controls/Pause Menu")
                .SetActive(isPaused);
            GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/6 - Controls/Reticle")
                .SetActive(!isPaused);
        }
    }
}