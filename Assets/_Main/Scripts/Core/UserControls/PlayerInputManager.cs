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

        public bool isDialogueInputActive;

        void Start()
        {
            isPaused = false;
            instance = this;
            isDialogueInputActive = true;
        }

        void Update()
        {
            if (isDialogueInputActive)
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

        public void EnableInput()
        {
            isInputActive = true;
        }

        public void DisableInput()
        {
            isInputActive = false;
        }

        public bool DefaultInput()
        {
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
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
                pauseMenu.OpenGeneralMenu();
            }
            else
            {
                MusicManager.instance.RaiseVolume();
                Time.timeScale = 1f;
                pauseMenu.CloseGeneralMenu();
            }
        }
    }
}