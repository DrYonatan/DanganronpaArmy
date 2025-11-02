using System.Collections;
using DIALOGUE;
using UnityEngine;


public class ComicManager : MonoBehaviour
{
    public enum ControlScheme
    {
        Scroll,
        Jump
    }
    public static ComicManager instance { get; private set; }

    public ComicUIAnimator animator;

    public AudioClip puzzleMusic;
    
    public ControlScheme controlScheme;

    private ComicSegment segment;

    public ScreenShatterManager screenShatter;

    private bool isInPuzzle;

    private Coroutine runningComicCoroutine;

    private ComicPage currentPresentedPage;
    

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isInPuzzle)
        {
            TrialCursorManager.instance.ReticleAsCursor();
            if (Input.GetMouseButton(1) && CheckQuestionPanelsPins())
            {
                StartCoroutine(PresentComic());
            }

            PagesControl();
            PinsControl();
            
        }
    }

    private void PinsControl()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && animator.firstPinNumber < animator.draggablePins.Count - 5)
        {
            animator.firstPinNumber++;
            animator.ScrollPinContainer();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && animator.firstPinNumber > 0)
        {
            animator.firstPinNumber--;
            animator.ScrollPinContainer();
        }
    }

    private void PagesControl()
    {
        if(controlScheme == ControlScheme.Jump)
            JumpControl();
        else if(controlScheme == ControlScheme.Scroll)
            ScrollControl();
    }

    private void JumpControl()
    {
        if (Input.GetKeyDown(KeyCode.A) && animator.pageNumber < animator.pageObjects.Count-1)
        {
            animator.JumpToNextPage();
        }
        else if (Input.GetKeyDown(KeyCode.D) && animator.pageNumber > 0)
        {
            animator.JumpToPrevPage();
        }
    }

    private void ScrollControl()
    {
        if (Input.GetKey(KeyCode.A) && animator.puzzlePagesContainer.localPosition.x < -900 + (animator.pageObjects.Count - 1) * animator.pageWidth)
        {
            animator.ScrollPuzzlePagesContainer(4f);

        }
        else if (Input.GetKey(KeyCode.D) && animator.puzzlePagesContainer.localPosition.x > -900)
        {
            animator.ScrollPuzzlePagesContainer(-4f);
        }
    }
    

    public void StartComicPuzzle(ComicSegment newSegment)
    {
        OverlayTextBoxManager.instance.SetAsTextBox();
        animator.gameObject.SetActive(true);
        segment = Instantiate(newSegment);
        animator.GeneratePuzzlePages(segment.pages);
        animator.GenerateComicPins(segment.availablePins);
        animator.SetPinsContainerStartPos();
        animator.UpdatePinsVisibility(0);
        SwitchToPuzzleMode();
    }

    private void SwitchToPuzzleMode()
    {
        MusicManager.instance.PlaySong(puzzleMusic);
        isInPuzzle = true;
        animator.ShowPuzzleUI();
        TrialCursorManager.instance.Show();
        DialogueSystem.instance.inputButton.gameObject.SetActive(false);
    }

    IEnumerator PresentComic()
    {
        MusicManager.instance.StopSong();
        TrialCursorManager.instance.Hide();
        isInPuzzle = false;
        animator.nowIUnderstand.gameObject.SetActive(true);
        yield return animator.nowIUnderstand.Show();
        animator.nowIUnderstand.gameObject.SetActive(false);
        animator.ShowSolutionUI();
        runningComicCoroutine = StartCoroutine(PlayComic());
        TrialCursorManager.instance.Hide();
        DialogueSystem.instance.inputButton.gameObject.SetActive(true);
    }
    
    IEnumerator PlayComic()
    {
        for (int i = 0; i < segment.pages.Count; i++)
        {
            currentPresentedPage = animator.GenerateSolutionPage(i);
            yield return currentPresentedPage.Play();
            Destroy(currentPresentedPage.gameObject);
        }

        screenShatter = Instantiate(screenShatter);
        yield return screenShatter.ScreenShatter();
        segment.Finish();
        animator.gameObject.SetActive(false);
    }

    private bool CheckQuestionPanelsPins()
    {
        foreach (ComicPage page in animator.pageObjects)
        {
            foreach(ComicPanel panel in page.panels)
            {
                if (!panel.IsReady())
                    return false;
            }
        }

        return true;
    }

    public void WrongAnswer()
    {
        TrialManager.instance.DecreaseHealth(1f);
        SwitchToPuzzleMode();
        Destroy(currentPresentedPage.gameObject);
        StopCoroutine(runningComicCoroutine);
    }

}