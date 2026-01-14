using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Game Structure/Trial Chapter Segment")]
public class TrialChapterSegment : ChapterSegment
{
    public List<TrialSegment> trialSegments;

    public override void Load()
    {
        TrialManager.instance.segments = trialSegments;
    }

    public override void LoadScene()
    {
        SceneManager.LoadScene("DebateScene");
    }
}
