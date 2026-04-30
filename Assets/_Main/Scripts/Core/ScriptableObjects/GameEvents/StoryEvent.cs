using DIALOGUE;

public class StoryEvent : GameEvent
{
    public VNConversationSegment conversation;
    
    public override void OnStart()
    {
        VNNodePlayer.instance.StartConversation(conversation);
    }

    public override void CheckIfFinished()
    {
        // Always finishes
        ProgressManager.instance.OnEventFinished();
    }

    public override void OnRoomLoad()
    {
    }
}
