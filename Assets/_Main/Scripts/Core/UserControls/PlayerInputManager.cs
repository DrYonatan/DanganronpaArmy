using DG.Tweening;
using UnityEngine;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance { get; private set; }

        public QuestionMarkShooter shooter;

        public MenuScreenContainer pauseMenu;

        public GameObject guideScreen;

        public bool guideOpen;

        public bool isPaused;

        public bool pauseAvailable;

        public bool isInputActive;

        public bool isDialogueInputActive;

        void Awake()
        {
            isPaused = false;
            instance = this;
            isDialogueInputActive = true;
            isInputActive = true;
            pauseAvailable = true;
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
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && !pauseMenu.isSubmenuOpen && isInputActive && pauseAvailable)
            {
                TogglePauseAndMenu();
            }

            if (Input.GetKeyDown(KeyCode.Escape) && isInputActive && !pauseMenu.isOpen && pauseAvailable)
            {
                ToggleGuide();
            }

            if (Input.GetKeyDown(KeyCode.Tab) && isInputActive && !pauseMenu.isOpen && DialogueSystem.instance.isActive)
            {
                DialogueSystem.instance.dialogueBoxAnimator.ToggleUI();
            }
        }

        private void ToggleGuide()
        {
            if (guideOpen)
                GuideDisappear();
            else
                GuideAppear();
        }

        private void GuideAppear()
        {
            guideOpen = true;
            TogglePause();
            guideScreen.SetActive(true);
            guideScreen.transform.DOKill();
            guideScreen.transform.localPosition = new Vector3(0, 1080, 0);
            guideScreen.transform.DOLocalMoveY(0f, 0.2f).SetUpdate(true);
        }

        private void GuideDisappear()
        {
            guideOpen = false;
            TogglePause();
            guideScreen.transform.DOKill();
            guideScreen.transform.localPosition = new Vector3(0, 0, 0);
            guideScreen.transform.DOLocalMoveY(1080, 0.2f).SetUpdate(true).OnComplete(() => guideScreen.SetActive(false));
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
            if (!CutSceneManager.instance.isPlaying && !isPaused && DialogueSystem.instance.dialogueBoxAnimator.uiEnabled)
            {
                DialogueSystem.instance.OnUserPrompt_Next();
            }
        }

        public void TogglePauseAndMenu()
        {
            TogglePause();
            if (isPaused)
            {
                pauseMenu.OpenGeneralMenu();
            }
            else
            {
                pauseMenu.CloseGeneralMenu();
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;

            Time.timeScale = isPaused ? 0f : 1f;
            if (isPaused)
                MusicManager.instance.LowerVolume();
            else
                MusicManager.instance.RaiseVolume();
        }
    }
}