using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

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

    public bool startEventImmediately = false; // used to know if to start the event as soon as the previous one ends or only after finish text

    public VNConversationSegment finishText;

    public List<GameEvent> conditionEvents;

    public VNConversationSegment unallowedText;

    public Dictionary<string, ObjectData> charactersData = new Dictionary<string, ObjectData>();

    public Dictionary<string, ObjectData> objectsData = new Dictionary<string, ObjectData>();

    public abstract void CheckIfFinished();

    public abstract void UpdateEvent();


    public abstract void PlayEvent();

    public virtual void OnFinish()
    {
        if (finishText != null)
        {
            VNDialogueManager.instance.StartConversation(finishText);

            finishText = null;
        }
    }

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