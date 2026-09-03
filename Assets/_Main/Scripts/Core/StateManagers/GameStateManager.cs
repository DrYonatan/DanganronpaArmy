using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance { get; private set; }

    public int chapterIndex;
    public int chapterSegmentIndex;
    public Dictionary<string, int> charactersRanks = new(); // Free time events ranks for each character

    public GameObject persistentObject;

    public ChaptersBank chaptersBank;

    public Camera sceneTransitionCamera;

    public UIState uiState = new UIState();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (persistentObject != null)
                DontDestroyOnLoad(persistentObject);

            GameObject prevTransitionCam = GameObject.Find("/Scene Transition Camera");
            if (prevTransitionCam != null)
                Destroy(prevTransitionCam);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (ProgressManager.instance != null)
        {
            InitializeVn();
        }

        else if (TrialManager.instance != null)
        {
            InitializeTrial();
        }
    }

    private void InitializeVn()
    {
        if (SaveManager.instance != null && SaveManager.instance.currentSaveSlot != -1)
        {
            StartCoroutine(ProgressManager.instance.LoadValuesFromSave());
        }
        else
        {
            ProgressManager.instance.StartNewGame();
        }
    }

    private void InitializeTrial()
    {
        if (SaveManager.instance != null && SaveManager.instance.currentSaveSlot != -1)
            TrialManager.instance.LoadValuesFromSave(SaveManager.instance.currentSaveSlot);
        else
        {
            TrialManager.instance.preTrialPrepMenu.Appear();
        }
    }

    private void StartNewSegment()
    {
        if (TrialManager.instance != null)
        {
            InitiateUIState();
            TrialManager.instance.preTrialPrepMenu.Appear();
        }

        else if (ProgressManager.instance != null)
        {
            persistentObject = GameObject.Find("Persistent");
            DontDestroyOnLoad(persistentObject);
            StartCoroutine(ProgressManager.instance.StartNewVnSegment());
        }
    }

    public void UpdateChapterIndexes(int newChapterIndex, int newChapterSegmentIndex)
    {
        chapterIndex = newChapterIndex;
        chapterSegmentIndex = newChapterSegmentIndex;
    }

    public void MoveToNextChapterSegment()
    {
        chapterSegmentIndex++;
        if(ProgressManager.instance != null)
           ProgressManager.instance.currentGameEventIndex = 0;
        MusicManager.instance.StopSong();

        if (chapterSegmentIndex < chaptersBank.chapters[chapterIndex].chapterSegments.Count)
        {
            StartCoroutine(MoveToNextChapterSegmentPipeline());
        }

        else
        {
            MoveToNextChapter();
        }
    }

    private IEnumerator MoveToNextChapterSegmentPipeline()
    {
        ImageScript.instance.FadeToBlack(0.2f);
        yield return new WaitForSeconds(1f);
        DOTween.KillAll();

        if (ProgressManager.instance != null)
        {
            ProgressManager.instance.currentGameEvent =
                (chaptersBank.chapters[chapterIndex].chapterSegments[chapterSegmentIndex] as VNChapterSegment)
                ?.gameEvents[0];
            WorldManager.instance.currentRoom = null;
        }
        bool pauseAvailable = PlayerInputManager.instance.pauseAvailable;

        if(chapterSegmentIndex == 0 || GetLastChapterSegment().saveAfter)
           yield return HandlePopup();

        sceneTransitionCamera.gameObject.SetActive(true);
        chaptersBank.chapters[chapterIndex].chapterSegments[chapterSegmentIndex].LoadScene();
        if (persistentObject != null)
            Destroy(persistentObject);
        yield return new WaitForSeconds(0.5f);
        PlayerInputManager.instance.pauseAvailable = pauseAvailable;

        StartNewSegment();
    }

    private IEnumerator HandlePopup()
    {
        ImageScript.instance.UnFadeToBlack(0f);
        if(ProgressManager.instance != null)
           ProgressManager.instance.savedInPopup = true;
        SavePopup popup = PlayerInputManager.instance.pauseMenu.generalMenu.savePopUp;
        popup.gameObject.SetActive(true);
        popup.finished = false;
        PlayerInputManager.instance.guideAvailable = false;
        PlayerInputManager.instance.pauseAvailable = false;
        yield return popup.WaitForCompletion();
        if(ProgressManager.instance != null)
           ProgressManager.instance.savedInPopup = false;
        ImageScript.instance.FadeToBlack(0f);
        yield return new WaitForSeconds(0.5f);
        PlayerInputManager.instance.guideAvailable = true;
        popup.gameObject.SetActive(false);
    }

    private void MoveToNextChapter()
    {
        chapterIndex++;
        chapterSegmentIndex = 0;

        if (chapterIndex < chaptersBank.chapters.Count)
        {
            StartCoroutine(MoveToNextChapterPipeline());
        }
        else
        {
            StartCoroutine(FinishGame());
        }
            
    }

    private IEnumerator FinishGame()
    {
        yield return HandlePopup();
        yield return ThankYouForPlaying();
    }
    public IEnumerator ThankYouForPlaying()
    {
        Time.timeScale = 1f;
        DOTween.KillAll();
        SceneManager.LoadScene("_Main/Scenes/ThankYouForPlaying");
        yield return new WaitForSeconds(0.1f);
        Destroy(persistentObject);
        Destroy(gameObject); 
    }

    private IEnumerator MoveToNextChapterPipeline()
    {
        yield return MoveToNextChapterSegmentPipeline();
        VNUIAnimator.instance.chapterNameText.text = chaptersBank.chapters[chapterIndex].chapterName;
    }

    public Chapter GetCurrentChapter()
    {
        if (chapterIndex < chaptersBank.chapters.Count)
            return chaptersBank.chapters[chapterIndex]; 
        return null;
    }

    public ChapterSegment GetCurrentChapterSegment()
    {
        return GetCurrentChapter()?.chapterSegments[chapterSegmentIndex];
    }
    
    public ChapterSegment GetLastChapterSegment()
    {
        return GetCurrentChapter()?.chapterSegments[chapterSegmentIndex-1];
    }

    public void ResetChapters()
    {
        chapterIndex = 0;
        chapterSegmentIndex = 0;
    }

    public void ResetUIState()
    {
        uiState.backgroundImage = new ImageState();
    }

    public void SetUIState(UIState state)
    {
        uiState = state;
    }

    public void InitiateUIState()
    {
        ImageScript.instance.overlayImage.sprite = Resources.Load<Sprite>($"Images/{uiState.overlayImage.spriteId}");
        ImageScript.instance.canvasGroup.DOFade(uiState.overlayImage.visible ? 1f : 0f, 0f);
        if (uiState.overlayImage.visible)
        {
            DialogueSystem.instance.SetTextBox(ImageScript.instance.overlayTextBoxAnimator);
            DialogueSystem.instance.TextBoxAppear();
        }

        ImageScript.instance.blackFadeUnderTextBox?.DOFade(uiState.underTextboxBlackFade.visible ? 1f : 0f, 0f);

        ImageScript.instance.background.sprite = Resources.Load<Sprite>($"Images/{uiState.backgroundImage.spriteId}");
        ImageScript.instance.background.DOFade(uiState.backgroundImage.visible ? 1f : 0f, 0f);

        VNAnimatedImage animatedImage =
            Resources.Load<VNAnimatedImage>($"Images/Animated/{uiState.animatedImage.prefabId}");
        if (animatedImage != null && uiState.animatedImage.visible)
        {
            ImageScript.instance.animatedImage = Instantiate(animatedImage);
            ImageScript.instance.animatedImageContainer.DOFade(1f, 0f).SetEase(Ease.Linear);
        }

        foreach (BackgroundCharacterState state in uiState.characterStates)
        {
            ImageScript.instance.CreateCharacterOnBackground(state.character);
            if (state.visible)
                ImageScript.instance.ShowCharacterOnBackground(state.character);
        }

        if (!uiState.namePlateVisible)
        {
            DialogueSystem.instance.HideNamePlate();
        }
    }
    
    public IEnumerator GoToTitleScreenPipeline()
    {
        ImageScript.instance.FadeToBlack(0.5f);
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        DOTween.KillAll();
        Destroy(SaveManager.instance.gameObject);
        SceneManager.LoadScene("TitleScreen");
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(persistentObject);
        Destroy(gameObject);
    }
}