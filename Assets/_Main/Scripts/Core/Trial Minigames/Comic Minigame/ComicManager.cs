using System.Collections;
using UnityEngine;

public class ComicManager : MonoBehaviour
{
    public static ComicManager instance { get; private set; }

    public RectTransform comicTransform;

    private ComicSegment segment;

    void Awake()
    {
        instance = this;
    }

    public void StartComic(ComicSegment newSegment)
    {
        segment = Instantiate(newSegment);
        segment.pages[0] = Instantiate(segment.pages[0], comicTransform);
        StartCoroutine(PresentComic());
    }

    IEnumerator PresentComic()
    {
        foreach (ComicPage comicPage in segment.pages)
        {
            yield return comicPage.Play();
        }
    }
}
