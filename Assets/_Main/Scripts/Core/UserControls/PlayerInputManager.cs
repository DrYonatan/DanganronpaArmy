using UnityEngine;
using UnityEngine.Serialization;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance {get; private set;}

        public QuestionMarkShooter shooter;

        public MenuScreenContainer pauseMenu;

        public bool isPaused;

        public bool isInputActive;

        void Start()
        {
            isPaused = false;
            instance = this;
        }
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
            
            if (Input.GetKeyDown(KeyCode.Alpha1) && !pauseMenu.isSubmenuOpen && isInputActive)
            {
                TogglePause();
            }
        }

        public void PromptAdvance()
        {
            if (!CutSceneManager.instance.isPlaying && !isPaused)
            {
                DialogueSystem.instance.OnUserPrompt_Next();
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                MusicManager.instance.LowerVolume();
                Time.timeScale = 0f;
                CursorManager.instance.Hide();
                pauseMenu.OpenGeneralMenu();
            }
            else
            {
                MusicManager.instance.RaiseVolume();
                Time.timeScale = 1f;
                CursorManager.instance.Show();
                pauseMenu.CloseGeneralMenu();
            }
        }

       
    }
}