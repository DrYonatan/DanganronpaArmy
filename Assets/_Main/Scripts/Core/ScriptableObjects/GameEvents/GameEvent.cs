using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent: ScriptableObject
{
    public bool isFinished;
    public TimeOfDay timeOfDay;
    public List<RoomData> roomDatas;
    public Room startRoom;
    public bool alwaysDoLoadAnimation;
    public bool stopPreviousMusic;
    public AudioClip startMusic;
    public abstract void OnStart();

    public abstract void CheckIfFinished();
    
    public abstract void OnRoomStartLoad();

    public abstract void OnRoomFinishLoad();
    
    public virtual void LoadSave(SaveData data)
    {
        VNConversationSegment currentConversation = ProgressManager.instance.conversationDatabase.Get(data.currentConversation);
        isFinished = data.eventState.isFinished;
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

    public abstract EventState HandleSave();
    
    protected IEnumerator StartWithRoomLoad()
    {
        Room roomToLoad = startRoom != null ? startRoom : WorldManager.instance.currentRoom;

        if (roomToLoad != null && (WorldManager.instance.currentTime != timeOfDay ||
            roomToLoad != WorldManager.instance.currentRoom || alwaysDoLoadAnimation))
        {
            WorldManager.instance.currentRoom = roomToLoad;
            yield return TimeOfDayManager.instance.ChangeTimeOfDay(timeOfDay);
            
            if(stopPreviousMusic)
                MusicManager.instance.StopSong();
            
            if(startMusic != null)
                MusicManager.instance.PlaySong(startMusic);
            
            yield return WorldManager.instance.LoadRoom(WorldManager.instance.currentRoom, null);
        }
        else
        {
            ImageScript.instance.UnFadeToBlack(0.2f);

            if(stopPreviousMusic)
                MusicManager.instance.StopSong();
            
            if(startMusic != null)
                MusicManager.instance.PlaySong(startMusic);
        }
        
        OnRoomStartLoad();
        OnRoomFinishLoad();
        WorldManager.instance.charactersObject?
            .AnimateCharacters();
    }
}
