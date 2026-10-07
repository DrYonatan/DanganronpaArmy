using UnityEngine;

public abstract class TrialSegment : ScriptableObject
{
    public abstract void Play();
    public int mistakes;
    protected virtual int CalculateScore()
    {
        float leftTime = TimerManager.instance.timer;
        return (int)(leftTime / 10 + Mathf.Max(0, 50 - mistakes * 10));
    }
    public virtual void Finish()
    {
        TrialManager.instance.OnSegmentFinished();
        if (this is DiscussionSegment) return;
        UserDataManager.instance.UpdateUserScore(CalculateScore());
    }

    public abstract void HandleGameOver();

}