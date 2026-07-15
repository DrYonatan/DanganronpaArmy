using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEditor;

public class StartFreeTimeConversation : Command
{
    public Character character;
    public override IEnumerator Execute()
    {
        CharacterFreeTimeEvents characterEvents =
            GameStateManager.instance.chaptersBank.charactersFreeTimeEventContainer.charactersEvents.Find((freeTime) =>
                freeTime.character == character);
        
        Dictionary<string, int> charactersRanks = GameStateManager.instance.charactersRanks;
        charactersRanks.TryAdd(character.name, 0);

        VNConversationSegment currentFreeTimeEvent = characterEvents.events[
            charactersRanks[
                character.name]];

        charactersRanks[character.name]++;

        ((WorldEvent)ProgressManager.instance.currentGameEvent).isFinished = true;
        
        VNNodePlayer.instance.AddToQueue(currentFreeTimeEvent);
        DialogueSystem.instance.TurnOnSingleTimeAuto();
        yield return null;
    }
    
    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        character = (Character)EditorGUILayout.ObjectField("Character", character, typeof(Character), false);
    }
    #endif
}