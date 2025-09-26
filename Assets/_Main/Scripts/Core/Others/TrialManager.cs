using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEngine;

public class TrialManager : MonoBehaviour
{
    public static TrialManager instance { get; private set; }
    public List<TrialSegment> segments = new List<TrialSegment>();
    private int currentIndex = 0;
    public List<CharacterStand> characterStands;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        TrialSegment segment = Instantiate(segments[currentIndex]);
        segment.Play();
    }

    public void OnSegmentFinished()
    {
        currentIndex++;
        TrialSegment segment = Instantiate(segments[currentIndex]);
        segment.Play();
    }
}
