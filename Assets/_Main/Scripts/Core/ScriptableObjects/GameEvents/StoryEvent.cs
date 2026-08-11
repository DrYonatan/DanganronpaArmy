
using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Story Event")]
public class StoryEvent : GameEvent
{
    public VNConversationSegment conversation;
    
    public override void OnStart()
    {
        WorldManager.instance.StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        ImageScript.instance.UnFadeToBlack(0.1f);
        yield return StartWithRoomLoad();
        VNNodePlayer.instance.StartConversation(conversation);
    }

    public override void CheckIfFinished()
    {
        // Always finishes
        ProgressManager.instance.OnEventFinished();
    }

    public override void OnRoomStartLoad()
    {
        if (WorldManager.instance.characterPanel == null)
            return;
        
        RoomData currentRoomData = roomDatas
            .First(roomData => roomData.room.roomName.Equals(WorldManager.instance.currentRoom.roomName));
        
        if (currentRoomData.worldObjects != null)
        {
            WorldObjectsParent ob = Instantiate(currentRoomData.worldObjects, WorldManager.instance.characterPanel.transform);
            ob.name = "Objects";
            ob.gameObject.SetActive(true);
        }
    }

    public override void OnRoomFinishLoad()
    {
        if (WorldManager.instance.characterPanel == null)
            return;
        
        RoomData currentRoomData = roomDatas
            .First(roomData => roomData.room.roomName.Equals(WorldManager.instance.currentRoom.roomName));
        
        if (currentRoomData.characters != null)
        {
            WorldCharactersParent ob = Instantiate(currentRoomData.characters, WorldManager.instance.characterPanel.transform);
            ob.name = "Characters";
            ob.gameObject.SetActive(true);
        }
    }

    public override EventState HandleSave()
    {
        return new EventState(isFinished);
    }
}
