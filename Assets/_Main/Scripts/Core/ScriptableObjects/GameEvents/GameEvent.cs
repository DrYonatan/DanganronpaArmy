using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent: ScriptableObject
{
    public TimeOfDay timeOfDay;
    public List<RoomData> roomDatas;

    public abstract void OnStart();

    public abstract void CheckIfFinished();
    
    public abstract void OnRoomLoad();
    
    public virtual void LoadSave(SaveData data)
    {
        
    }
}
