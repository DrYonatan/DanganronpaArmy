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
}
