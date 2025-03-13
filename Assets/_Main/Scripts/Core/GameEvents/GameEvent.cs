using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : ScriptableObject
{
    public bool isFinished;

    abstract public void CheckIfFinished();

    abstract public void UpdateEvent();

    abstract public bool CheckIfToPlay();

    abstract public void PlayEvent();

    abstract public void OnFinish();
}
