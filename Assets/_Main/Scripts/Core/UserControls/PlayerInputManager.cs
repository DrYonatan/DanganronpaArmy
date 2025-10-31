using UnityEngine;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance {get; private set;}

        public QuestionMarkShooter shooter;

        public PauseMenuManager pauseMenu;

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
            if (!CutSceneManager.instance.isPlaying)
            {
                DialogueSystem.instance.OnUserPrompt_Next();
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                CursorManager.instance.Hide();
                pauseMenu.OpenMenu();
            }
            else
            {
                CursorManager.instance.Show();
                pauseMenu.CloseMenu();
            }
        }

       
    }
}