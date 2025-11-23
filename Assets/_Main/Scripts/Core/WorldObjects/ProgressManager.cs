using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance { get; private set; }
    public List<GameEvent> gameEvents;

    public CharactersFreeTimeEventsSO characterEventsAsset;
    public Dictionary<string, int> charactersRanks = new(); // Free time events ranks for each character

    public GameEvent currentGameEvent;
    public int currentGameEventIndex;

    ConversationDatabase conversationDatabase;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);
        currentGameEvent.OnStart();
    }

    public void OnEventFinished()
    {
        currentGameEventIndex++;
        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);
        currentGameEvent.OnStart();
    }

    public void SaveGame(int slot)
    {
        SaveData data = new SaveData(currentGameEventIndex, WorldManager.instance.currentRoom.name,
            VNNodePlayer.instance.currentConversation?.guid, VNNodePlayer.instance.lineIndex, MusicManager.instance.audio.name,
            currentGameEvent.charactersData, currentGameEvent.objectsData, SceneManager.GetActiveScene().name,
            charactersRanks);
        SaveSystem.SaveGame(data, slot);
    }
}