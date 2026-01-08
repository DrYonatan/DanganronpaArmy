using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Game Structure/VN Chapter Segment")]
public class VNChapterSegment: ScriptableObject
{
    public List<GameEvent> gameEvents;
}
