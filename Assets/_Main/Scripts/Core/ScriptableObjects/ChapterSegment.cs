using UnityEngine;

public abstract class ChapterSegment : ScriptableObject
{
    public abstract void Load();

    public abstract void LoadScene();

    public abstract string GetSceneName();
}