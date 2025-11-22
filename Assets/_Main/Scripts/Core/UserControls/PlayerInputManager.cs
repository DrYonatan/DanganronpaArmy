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

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SaveData data = new SaveData();
                data.gameEventIndex = ProgressManager.instance.currentGameEventIndex;
                SaveSystem.SaveGame(data, 1);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SaveData data = new SaveData();
                data.gameEventIndex = ProgressManager.instance.currentGameEventIndex;
                SaveSystem.SaveGame(data, 2);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                ProgressManager.instance.currentGameEventIndex = SaveSystem.LoadGame(1).gameEventIndex;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ProgressManager.instance.currentGameEventIndex = SaveSystem.LoadGame(2).gameEventIndex;
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
                MusicManager.instance.LowerVolume();
                Time.timeScale = 0f;
                CursorManager.instance.Hide();
                pauseMenu.OpenMenu();
            }
            else
            {
                MusicManager.instance.RaiseVolume();
                Time.timeScale = 1f;
                CursorManager.instance.Show();
                pauseMenu.CloseMenu();
            }
        }

       
    }
}