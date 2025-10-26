using System.Collections;
using DIALOGUE;
using UnityEngine;

public class ComicManager : MonoBehaviour
{
    public static ComicManager instance { get; private set; }

    public RectTransform comicTransform;

    public ScreenShatterManager screenShatter;
    
    private ComicSegment segment;
    
    void Awake()
    {
        instance = this;
    }

    public void PresentComic(ComicSegment newSegment)
    {
        comicTransform.gameObject.SetActive(true);
        segment = Instantiate(newSegment);
        OverlayTextBoxManager.instance.SetAsTextBox();
        StartCoroutine(PlayComic());
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
