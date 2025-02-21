using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : ScriptableObject
{
    abstract public bool CheckIfFinished();

    abstract public void UpdateEvent();

    abstract public bool CheckIfToPlay();

    abstract public void PlayEvent();

    abstract public void OnFinish();
}
