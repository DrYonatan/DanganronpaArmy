using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : ScriptableObject
{
    public bool isFinished;

    public List<GameEvent> conditionEvents;

    public TextAsset unallowedText;

    abstract public void CheckIfFinished();

    abstract public void UpdateEvent();

    
    abstract public void PlayEvent();

    abstract public void OnFinish();

    public bool CheckIfToPlay()
    {
        bool playScene = true;
        if (conditionEvents != null)
            foreach (GameEvent conditionEvent in conditionEvents)
            {
                if (!conditionEvent.isFinished)
                    playScene = false;

            }
        
        return playScene;
    }
}
