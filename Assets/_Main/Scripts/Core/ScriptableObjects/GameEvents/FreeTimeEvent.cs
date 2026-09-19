
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Free Time Event")]
public class FreeTimeEvent : WorldEvent
{
    public Character currentCharacter;

    public override void OnStart()
    {
        WorldManager.instance.StartCoroutine(OnStartRoutine());
    }

    protected override IEnumerator OnStartRoutine()
    {
        yield return base.OnStartRoutine();
        
    }

    public override void CheckIfFinished()
    {
        if(isFinished)
            OnFinish();
        else
           OnNotFinished();
    }
}