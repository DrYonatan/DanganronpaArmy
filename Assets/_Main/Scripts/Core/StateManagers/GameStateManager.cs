using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance { get; private set; }

    public int chapterIndex;
    public int chapterSegmentIndex;
    public Dictionary<string, int> charactersRanks = new(); // Free time events ranks for each character

    public GameObject persistentObject;
    
    public List<Chapter> chapters;

    public Camera sceneTransitionCamera;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (persistentObject != null)
                DontDestroyOnLoad(persistentObject);
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
            StartCoroutine(ProgressManager.instance.LoadValuesFromSave());
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
            TrialManager.instance.StartNewTrial();
        }
    }

    private void StartNewSegment()
    {
        if (TrialManager.instance != null)
        {
            TrialManager.instance.StartNewTrial();
        }

        else if(ProgressManager.instance != null)
        {
            persistentObject = GameObject.Find("Persistent");
            DontDestroyOnLoad(persistentObject);
            StartCoroutine(ProgressManager.instance.StartNewVnSegment());
        }
    }

    public void MoveToNextChapterSegment()
    {
        StartCoroutine(MoveToNextChapterSegmentPipeline());
    }

    private IEnumerator MoveToNextChapterSegmentPipeline()
    {
        ImageScript.instance.FadeToBlack(0.2f);
        yield return new WaitForSeconds(1f);
        sceneTransitionCamera.gameObject.SetActive(true);
        chapterSegmentIndex++;
        chapters[chapterIndex].chapterSegments[chapterSegmentIndex].LoadScene();
        if(persistentObject != null) 
            Destroy(persistentObject);
        yield return new WaitForSeconds(0.5f);
        StartNewSegment();
    }

    public void ResetChapters()
    {
        chapterIndex = 0;
        chapterSegmentIndex = 0;
    }
}