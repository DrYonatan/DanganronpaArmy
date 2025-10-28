using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Comic Segment")]
public class ComicSegment : TrialSegment
{
    public List<ComicPage> pages = new List<ComicPage>();
    public List<ComicPin> availablePins = new List<ComicPin>();
    public override void Play()
    {
        ComicManager.instance.StartComicPuzzle(this);
    }
}
