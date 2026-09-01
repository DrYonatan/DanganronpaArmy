using UnityEngine;

public abstract class ChapterSegment : ScriptableObject
{
    public bool saveAfter = true;
    public abstract void Load();

    public abstract void LoadScene();

    public abstract string GetSceneName();
}