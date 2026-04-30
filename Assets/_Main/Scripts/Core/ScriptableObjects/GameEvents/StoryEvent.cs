
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Story Event")]
public class StoryEvent : GameEvent
{
    public VNConversationSegment conversation;
    
    public override void OnStart()
    {
        ImageScript.instance.UnFadeToBlack(0.1f);
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
