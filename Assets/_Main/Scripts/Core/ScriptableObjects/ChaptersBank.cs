using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Chapters Bank")]
public class ChaptersBank : ScriptableObject
{
    public List<Chapter> chapters;
}
