using UnityEngine;

public abstract class TrialSegment : ScriptableObject
{
    public abstract void Play();

    public virtual void Finish()
    {
        TrialManager.instance.OnSegmentFinished();
    }

    public abstract void HandleGameOver();

}