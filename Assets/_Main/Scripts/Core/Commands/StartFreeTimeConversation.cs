using System.Collections;
using System.Collections.Generic;
public class StartFreeTimeConversation : Command
{
    public override IEnumerator Execute()
    {
        Character character = ((FreeTimeEvent)(ProgressManager.instance.currentGameEvent)).currentCharacter;

        CharacterFreeTimeEvents characterEvents =
            GameStateManager.instance.chaptersBank.charactersFreeTimeEventContainer.charactersEvents.Find((freeTime) =>
                freeTime.character == character);
        
        Dictionary<string, int> charactersRanks = GameStateManager.instance.charactersRanks;
        charactersRanks.TryAdd(character.name, 0);

        VNConversationSegment currentFreeTimeEvent = characterEvents.events[
            charactersRanks[
                character.name]];

        ((WorldEvent)ProgressManager.instance.currentGameEvent).isFinished = true;
        
        VNNodePlayer.instance.AddToQueue(currentFreeTimeEvent);
        yield return null;
    }
}