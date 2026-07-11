using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent: ScriptableObject
{
    public TimeOfDay timeOfDay;
    public List<RoomData> roomDatas;
    public Room startRoom;
    public AudioClip startMusic;
    public abstract void OnStart();

    public abstract void CheckIfFinished();
    
    public abstract void OnRoomLoad();
    
    public virtual void LoadSave(SaveData data)
    {
        VNConversationSegment currentConversation = ProgressManager.instance.conversationDatabase.Get(data.currentConversation);
        if (currentConversation != null)
        {
            VNNodePlayer.instance.lineIndex = data.currentLineIndex;
            VNNodePlayer.instance.StartConversation(currentConversation);
            CameraManager.instance.initialRotation = Quaternion.Euler(new Vector3(data.conversationInitialRotation[0],
                data.conversationInitialRotation[1], data.conversationInitialRotation[2]));
        }
        else
        {
            CursorManager.instance.Show();
        }
        
        ImageScript.instance.UnFadeToBlack(0.1f);
    }
    
    protected IEnumerator StartWithRoomLoad()
    {
        Room roomToLoad;

        if (startRoom != null &&
            WorldManager.instance.currentRoom?.roomName != startRoom.roomName)
            roomToLoad = startRoom;
        else
        {
            roomToLoad = WorldManager.instance.currentRoom;
        }

        if (roomToLoad != null && WorldManager.instance.currentTime != timeOfDay ||
            roomToLoad != WorldManager.instance.currentRoom)
        {
            WorldManager.instance.currentRoom = roomToLoad;
            yield return TimeOfDayManager.instance.ChangeTimeOfDay(timeOfDay);
            
            if(startMusic != null)
                MusicManager.instance.PlaySong(startMusic);
            
            yield return WorldManager.instance.LoadRoom(WorldManager.instance.currentRoom, null);
        }

        OnRoomLoad();
        WorldManager.instance.charactersObject?
            .AnimateCharacters();
    }
}
