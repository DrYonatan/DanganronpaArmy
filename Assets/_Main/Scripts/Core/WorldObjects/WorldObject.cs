using UnityEngine;

public class WorldObject : ConversationInteractable
{
    protected override void FinishInteraction()
    {
        base.FinishInteraction();
        
        CharacterClickEffects.instance.MakeCharactersDisappear(WorldManager.instance.charactersObject, 1f);

        if (ProgressManager.instance.currentGameEvent != null)
        {
            ((WorldEvent)ProgressManager.instance.currentGameEvent).objectsData[id] = new ObjectData(isClicked, clickCount);
        }
    }
}