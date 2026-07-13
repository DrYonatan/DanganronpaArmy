
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Free Time Event")]
public class FreeTimeEvent : WorldEvent
{
    public Character currentCharacter;
    protected void OnFinish()
    {
        base.OnFinish();
    }

    public override void CheckIfFinished()
    {
        if(isFinished)
            OnFinish();
        else
           OnNotFinished();
    }
}