using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Comic Segment")]
public class ComicSegment : TrialSegment
{
    public List<ComicPage> pages = new List<ComicPage>();
    public override void Play()
    {
        ComicManager.instance.StartComic(this);
    }
}
