using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Destination Event")]
public class DestinationEvent : GameEvent
{
    public Room targetRoom;

    public override void CheckIfFinished()
    {
        if (WorldManager.instance.currentRoom.roomName.Equals(targetRoom.roomName))
        {
            isFinished = true;
        }
        
        if(isFinished)
           OnFinish();
    }
}