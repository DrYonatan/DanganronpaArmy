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

[System.Serializable]
public class RoomData
{
    public Room room;
    public GameObject characters;
    public GameObject worldObjects;
}

public abstract class GameEvent : ScriptableObject
{
    public bool isFinished;

    public bool startEventImmediately = false; // used to know if to start the event as soon as the previous one ends or only after finish text

    public VNConversationSegment finishText;
    
    public VNConversationSegment unallowedText;

    public List<RoomData> roomDatas;

    public Dictionary<string, ObjectData> charactersData = new Dictionary<string, ObjectData>();

    public Dictionary<string, ObjectData> objectsData = new Dictionary<string, ObjectData>();

    public abstract void CheckIfFinished();

    public abstract void OnStart();

    protected virtual void OnFinish()
    {
        if (finishText != null)
        {
            VNNodePlayer.instance.StartConversation(finishText);
            finishText = null;
        }
        
        ProgressManager.instance.OnEventFinished();
    }
}