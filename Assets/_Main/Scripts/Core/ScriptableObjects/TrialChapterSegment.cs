using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Game Structure/Trial Chapter Segment")]
public class TrialChapterSegment : ScriptableObject
{
    public List<TrialSegment> trialSegments;
}
