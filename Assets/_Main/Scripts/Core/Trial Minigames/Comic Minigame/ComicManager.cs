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

    private bool isReadyToPresent;

    private Coroutine runningComicCoroutine;

    private ComicPage currentPresentedPage;

    public int currentPageIndex;
    

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isInPuzzle)
        {
            TrialCursorManager.instance.ReticleAsCursor();
            if (Input.GetMouseButton(1) && isReadyToPresent)
            {
                StartCoroutine(PresentComic());
            }

            PagesControl();
            PinsControl();
            
        }
    }

    public void StartMiniGame(ComicSegment newSegment)
    {
        segment = Instantiate(newSegment);
        StartCoroutine(StartPipeline());
    }

    private IEnumerator StartPipeline()
    {
        ImageScript.instance.FadeToBlack(0.1f);
        yield return new WaitForSeconds(0.5f);
        ImageScript.instance.UnFadeToBlack(0.1f);
        animator.gameObject.SetActive(true);
        yield return animator.Intro();
        StartComicPuzzle();
    }

    public void UpdateIsReadyToPresent()
    {
        isReadyToPresent = CheckQuestionPanelsPins();
        if(isReadyToPresent)
            animator.BlinkReEnact();
        else
            animator.StopBlinkingReEnact();
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
    

    private void StartComicPuzzle()
    {
        OverlayTextBoxManager.instance.SetAsTextBox();
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
        animator.StopBlinkingReEnact();
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
        for (int i = currentPageIndex; i < segment.pages.Count; i++)
        {
            currentPresentedPage = animator.GenerateSolutionPage(i);
            currentPageIndex = i;
            yield return currentPresentedPage.Play();
            Destroy(currentPresentedPage.gameObject);
        }

        screenShatter = Instantiate(screenShatter);
        yield return screenShatter.ScreenShatter();
        segment.Finish();
        animator.gameObject.SetActive(false);
    }

    public void LockPin()
    {
        ComicQuestionPanel panel = animator.pageObjects[currentPageIndex]
            .panels[currentPresentedPage.currentPanelIndex] as ComicQuestionPanel;
        if (panel != null)
        {
            ComicDraggablePin pin = panel.selectedPin;
            pin.Lock();
        }
    }

    public void RemovePinFromPanel()
    {
        ComicQuestionPanel panel = animator.pageObjects[currentPageIndex]
            .panels[currentPresentedPage.currentPanelIndex] as ComicQuestionPanel;

        if (panel != null)
        {
            panel.selectedPin.ResetParent();
            panel.selectedPin = null;
        }
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
    
    public IEnumerator WrongAnswer()
    {
        Stop();
        
        OverlayTextBoxManager.instance.Show();
        TrialManager.instance.barsAnimator.ShowGlobalBars(0.2f);
        
        TrialManager.instance.DecreaseHealth(1f);
        foreach (DialogueNode node in UtilityNodesRuntimeBank.instance.nodesCollection.wrongComicNodes)
        {
            yield return DialogueSystem.instance.Say(node);
        }
        
        SwitchToPuzzleMode();
        Destroy(currentPresentedPage.gameObject);
        OverlayTextBoxManager.instance.Hide();
        TrialManager.instance.barsAnimator.HideGlobalBars(0.2f);
    }

    private void Stop()
    {
      StopCoroutine(runningComicCoroutine);
      currentPresentedPage.Stop();
    }
}