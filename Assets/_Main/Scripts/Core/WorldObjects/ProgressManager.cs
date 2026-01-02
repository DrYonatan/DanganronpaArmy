using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public ConversationDatabase conversationDatabase;

    public GameObject persistentObject;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(persistentObject);
    }

    void Start()
    {
        if (SaveManager.instance != null && SaveManager.instance.currentSaveSlot != -1)
            StartCoroutine(LoadValuesFromSave());
        else
        {
            StartCoroutine(StartNewGame());
        }
    }

    public void OnEventFinished()
    {
        StartCoroutine(MoveToNextEvent());
    }

    private IEnumerator MoveToNextEvent()
    {
        currentGameEventIndex++;
        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);

        if (WorldManager.instance.currentTime != currentGameEvent.timeOfDay)
        {
            yield return TimeOfDayManager.instance.ChangingTime(currentGameEvent.timeOfDay);
            WorldManager.instance.StartLoadingRoom(WorldManager.instance.currentRoom, null);
        }
        
        else
           currentGameEvent.OnStart();
    }

    private IEnumerator LoadValuesFromSave()
    {
        WorldManager.instance.isLoading = true;
        SaveData data = SaveManager.instance != null ? SaveManager.instance.LoadCurrentSave() : SaveSystem.LoadGame(1);
        
        yield return TimeOfDayManager.instance.ChangingTime(data.timeOfDay);;
        
        currentGameEventIndex = data.gameEventIndex;
        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);
        WorldManager.instance.currentRoom = Resources.Load<Room>($"Rooms/{data.currentRoom}");
        MusicManager.instance.PlaySong(Resources.Load<AudioClip>($"Audio/Music/{data.currentMusic}"));
        currentGameEvent.charactersData = data.charactersData.ToDictionary(c => c.key, c => c.value);
        currentGameEvent.objectsData = data.objectsData.ToDictionary(c => c.key, c => c.value);

        CameraManager.instance.cameraTransform.position =
            new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        CameraManager.instance.cameraTransform.rotation =
            Quaternion.Euler(new Vector3(data.playerRotation[0], data.playerRotation[1], data.playerRotation[2]));

        VNConversationSegment currentConversation = conversationDatabase.Get(data.currentConversation);
        if (currentConversation != null)
        {
            VNNodePlayer.instance.lineIndex = data.currentLineIndex;
            VNNodePlayer.instance.StartConversation(currentConversation);
            CameraManager.instance.initialRotation = Quaternion.Euler(new Vector3(data.conversationInitialRotation[0],
                data.conversationInitialRotation[1], data.conversationInitialRotation[2]));
        }

        WorldManager.instance.Initialize();
    }

    private IEnumerator StartNewGame()
    {
        yield return TimeOfDayManager.instance.ChangingTime(gameEvents[0].timeOfDay);
        currentGameEvent = Instantiate(gameEvents[0]);
        WorldManager.instance.StartLoadingRoom(WorldManager.instance.currentRoom, null);
    }
}