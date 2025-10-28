using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicUIAnimator : MonoBehaviour
{
    public RectTransform questionPanelsSpawnLocation;
    
    public CanvasGroup puzzleCanvasGroup;
    public CanvasGroup solutionCanvasGroup;

    public void ShowPuzzleUI()
    {
        puzzleCanvasGroup.alpha = 1f;
        solutionCanvasGroup.alpha = 0f;
    }

    public void ShowSolutionUI()
    {
        puzzleCanvasGroup.alpha = 0f;
        solutionCanvasGroup.alpha = 1f;
    }
    
}
