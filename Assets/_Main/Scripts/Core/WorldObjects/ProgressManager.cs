using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    void Start()
    {
        LoadValuesFromSave();
    }

    public void OnEventFinished()
    {
        currentGameEventIndex++;
        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);
        currentGameEvent.OnStart();
    }

    private void LoadValuesFromSave()
    {
        WorldManager.instance.isLoading = true;
        SaveData data = SaveManager.instance != null ? SaveManager.instance.LoadCurrentSave() : SaveSystem.LoadGame(1);
        currentGameEventIndex = data.gameEventIndex;
        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);
        WorldManager.instance.currentRoom = Resources.Load<Room>($"Rooms/{data.currentRoom}");
        MusicManager.instance.PlaySong(Resources.Load<AudioClip>($"Audio/Music/{data.currentMusic}"));
        currentGameEvent.charactersData = data.charactersData.ToDictionary(c => c.key, c => c.value);
        currentGameEvent.objectsData = data.objectsData.ToDictionary(c => c.key, c => c.value);

        WorldManager.instance.Initialize();
        currentGameEvent.OnStart();
    }
}