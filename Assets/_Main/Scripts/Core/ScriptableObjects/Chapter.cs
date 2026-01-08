using UnityEngine;

[CreateAssetMenu(menuName = "Data/Game Structure/Chapter")]
public class Chapter : ScriptableObject
{
    public string chapterName;
    public VNChapterSegment vnSegment;
    public TrialChapterSegment trialSegment;
}
