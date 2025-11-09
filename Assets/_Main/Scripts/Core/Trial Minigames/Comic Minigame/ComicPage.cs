using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicPage : MonoBehaviour
{
    public RectTransform rectTransform;
    [SerializeReference] public List<ComicPanel> panels = new List<ComicPanel>();
    private Coroutine runningPageCoroutine;
    private bool isDone;
    public int currentPanelIndex;

    
    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public IEnumerator Play()
    {
        foreach (ComicPanel panel in panels)
        {
            panel.StartUpAnimation();
        }
        
        runningPageCoroutine = StartCoroutine(PlayPanels());

        yield return new WaitUntil(() => isDone);
    }

    private IEnumerator PlayPanels()
    {
        foreach (ComicPanel panel in panels)
        {
            yield return panel.Play();
            currentPanelIndex++;
        }

        isDone = true;
    }
    
    public void Stop()
    {
        if(runningPageCoroutine != null)
           StopCoroutine(runningPageCoroutine);
        foreach (ComicPanel panel in panels)
        {
            panel.Stop();
        }
    }
    
}
