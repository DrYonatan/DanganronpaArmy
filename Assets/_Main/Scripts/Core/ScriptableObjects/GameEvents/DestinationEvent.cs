using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Destination Event")]
public class DestinationEvent : GameEvent
{
    public Room targetRoom;

    public override void CheckIfFinished()
    {
        if (WorldManager.instance.currentRoom.name.Equals(targetRoom.name))
        {
            isFinished = true;
        }
        
        if(isFinished)
           OnFinish();
    }
}