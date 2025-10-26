using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicPage : MonoBehaviour
{
    [SerializeReference] public List<ComicPanel> panels = new List<ComicPanel>();

    public IEnumerator Play()
    {
        foreach (ComicPanel panel in panels)
        {
            yield return panel.Play();
        }
    }
}
