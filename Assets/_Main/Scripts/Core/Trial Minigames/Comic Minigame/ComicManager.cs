using System.Collections;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;

public class ComicManager : MonoBehaviour
{
    public static ComicManager instance { get; private set; }

    public ComicUIAnimator animator;

    private ComicSegment segment;

    public ScreenShatterManager screenShatter;

    private bool isInPuzzle;

    public Coroutine runningComicCoroutine;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isInPuzzle)
        {
            TrialCursorManager.instance.ReticleAsCursor();
            if (Input.GetMouseButton(1))
            {
                PresentComic();
            }

            if (Input.GetKey(KeyCode.A) && animator.puzzlePagesContainer.transform.localPosition.x <
                animator.pagesContainerStartPos + (animator.pageObjects.Count - 1) * animator.pageWidth)
            {
                animator.ScrollPuzzlePagesContainer(4f);
            }
            else if (Input.GetKey(KeyCode.D) && animator.puzzlePagesContainer.transform.localPosition.x >
                     animator.pagesContainerStartPos)
            {
                animator.ScrollPuzzlePagesContainer(-4f);
            }
        }
    }

    public void StartComicPuzzle(ComicSegment newSegment)
    {
        OverlayTextBoxManager.instance.SetAsTextBox();
        animator.gameObject.SetActive(true);
        segment = Instantiate(newSegment);
        isInPuzzle = true;
        animator.GeneratePuzzlePages(segment.pages);
        animator.GenerateComicPins(segment.availablePins);
        SwitchToPuzzleMode();
    }

    public void SwitchToPuzzleMode()
    {
        animator.ShowPuzzleUI();
        TrialCursorManager.instance.Show();
        DialogueSystem.instance.inputButton.gameObject.SetActive(false);
    }

    void PresentComic()
    {
        isInPuzzle = false;
        animator.ShowSolutionUI();
        runningComicCoroutine = StartCoroutine(PlayComic());
        TrialCursorManager.instance.Hide();
        DialogueSystem.instance.inputButton.gameObject.SetActive(true);
    }

    IEnumerator PlayComic()
    {
        for (int i = 0; i < segment.pages.Count; i++)
        {
            segment.pages[i] = animator.GenerateSolutionPage(i);
            yield return segment.pages[i].Play();
            Destroy(segment.pages[i].gameObject);
        }

        screenShatter = Instantiate(screenShatter);
        yield return screenShatter.ScreenShatter();
        segment.Finish();
    }
}