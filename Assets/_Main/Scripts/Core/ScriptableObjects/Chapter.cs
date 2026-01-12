using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Game Structure/Chapter")]
public class Chapter : ScriptableObject
{
    public string chapterName;
    public List<ChapterSegment> chapterSegments;
}
