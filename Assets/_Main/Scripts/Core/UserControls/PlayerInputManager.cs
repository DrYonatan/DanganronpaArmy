using UnityEngine;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance { get; private set; }

        public QuestionMarkShooter shooter;

        public MenuScreenContainer pauseMenu;

        public bool isPaused;

        public bool isInputActive;

        void Start()
        {
            isPaused = false;
            instance = this;
            isInputActive = true;
        }

        void Update()
        {
            if (isInputActive)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                if (!isPaused)
                {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) ||
                        Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        PromptAdvance();
                    }

                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        DialogueSystem.instance.SetSkip(true);
                    }
                    else
                    {
                        DialogueSystem.instance.SetSkip(false);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha1) && !pauseMenu.isSubmenuOpen)
                {
                    TogglePause();
                }
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