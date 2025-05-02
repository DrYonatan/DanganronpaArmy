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

    public TextAsset finishText;

    public List<GameEvent> conditionEvents;

    public TextAsset unallowedText;

    public Dictionary<string, ObjectData> charactersData = new Dictionary<string, ObjectData>();

    public Dictionary<string, ObjectData> objectsData = new Dictionary<string, ObjectData>();

    public abstract void CheckIfFinished();

    public abstract void UpdateEvent();


    public abstract void PlayEvent();

    public virtual void OnFinish()
    {
        if (finishText != null)
        {
            List<string> lines = FileManager.ReadTextAsset(finishText);
            if (lines != null)
                DialogueSystem.instance.Say(lines);

            ProgressManager.instance.DecideWhichSceneToPlay();
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