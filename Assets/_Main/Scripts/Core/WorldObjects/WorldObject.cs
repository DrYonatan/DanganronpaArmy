public class WorldObject : ConversationInteractable
{
    public override void FinishInteraction()
    {
        base.FinishInteraction();

        if (ProgressManager.instance.currentGameEvent != null)
        {
            ProgressManager.instance.currentGameEvent.objectsData[name] = new ObjectData(isClicked);
        }
    }
}