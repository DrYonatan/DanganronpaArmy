public class WorldObject : ConversationInteractable
{
    protected override void FinishInteraction()
    {
        base.FinishInteraction();

        if (ProgressManager.instance.currentGameEvent != null)
        {
            ((WorldEvent)ProgressManager.instance.currentGameEvent).objectsData[name] = new ObjectData(isClicked);
        }
    }
}