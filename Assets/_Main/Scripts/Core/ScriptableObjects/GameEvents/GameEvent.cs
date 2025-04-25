using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData 
{
    public bool isClicked;

    public ObjectData(bool isClicked) 
    {
        this.isClicked = isClicked;
    }
}

public abstract class GameEvent : ScriptableObject
{
    public bool isFinished;

    public List<GameEvent> conditionEvents;

    public TextAsset unallowedText;

    public Dictionary<string, ObjectData> charactersData = new Dictionary<string, ObjectData>();

    public Dictionary<string, ObjectData> objectsData = new Dictionary<string, ObjectData>();

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
