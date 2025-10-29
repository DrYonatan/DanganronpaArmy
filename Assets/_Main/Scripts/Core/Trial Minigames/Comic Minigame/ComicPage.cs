using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicPage : MonoBehaviour
{
    public RectTransform rectTransform;
    [SerializeReference] public List<ComicPanel> panels = new List<ComicPanel>();
    
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
        
        foreach (ComicPanel panel in panels)
        {
            yield return panel.Play();
        }
    }
}
