using System.Collections;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;

public class ComicManager : MonoBehaviour
{
    public static ComicManager instance { get; private set; }

    public RectTransform comicTransform;

    public ScreenShatterManager screenShatter;

    public RectTransform questionPanelsSpawnLocation;
    
    private ComicSegment segment;
    
    public CanvasGroup puzzleCanvasGroup;
    public CanvasGroup solutionCanvasGroup;

    private bool isInPuzzle;
    
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(isInPuzzle)
           TrialCursorManager.instance.ReticleAsCursor();
    }

    public void StartComicPuzzle(ComicSegment newSegment)
    {
        
        OverlayTextBoxManager.instance.SetAsTextBox();
        comicTransform.gameObject.SetActive(true);
        segment = Instantiate(newSegment);
        isInPuzzle = true;
        SwitchToPuzzleMode();
    }

    public void SwitchToPuzzleMode()
    {
        puzzleCanvasGroup.alpha = 1f;
        solutionCanvasGroup.alpha = 0f;
        TrialCursorManager.instance.Show();
        DialogueSystem.instance.inputButton.gameObject.SetActive(false);
    }

    public void PresentComic()
    {
        isInPuzzle = false;
        puzzleCanvasGroup.alpha = 0f;
        solutionCanvasGroup.alpha = 1f;
        StartCoroutine(PlayComic());
        TrialCursorManager.instance.Hide();
        DialogueSystem.instance.inputButton.gameObject.SetActive(true);
    }

    IEnumerator PlayComic()
    {
        for (int i = 0; i < segment.pages.Count; i++)
        {
            segment.pages[i] = Instantiate(segment.pages[i], comicTransform);
            yield return segment.pages[i].Play();
            Destroy(segment.pages[i].gameObject);
        }

        screenShatter = Instantiate(screenShatter);
        yield return screenShatter.ScreenShatter();
        segment.Finish();
    }
}
