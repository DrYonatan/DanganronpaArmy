using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance { get; private set; }
    public List<GameEvent> gameEvents;

    public CharactersFreeTimeEventsSO characterEventsAsset;

    public GameEvent currentGameEvent;
    public int currentGameEventIndex;

    public ConversationDatabase conversationDatabase;

    private void Awake()
    {
        instance = this;
    }

    public void StartNewGame()
    {
        GameStateManager.instance.ResetChapters();
        StartCoroutine(StartNewVnSegment());
    }


    private void LoadGameEvents(ChapterSegment segment)
    {
        segment.Load();
    }

    public void OnEventFinished()
    {
        StartCoroutine(MoveToNextEvent());
    }

    private IEnumerator MoveToNextEvent()
    {
        currentGameEventIndex++;

        if (currentGameEventIndex >= gameEvents.Count)
        {
            GameStateManager.instance.MoveToNextChapterSegment();
            yield break;
        }

        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);

        Room roomToLoad;

        if (currentGameEvent.startRoom != null &&
            WorldManager.instance.currentRoom.roomName != currentGameEvent.startRoom.roomName)
            roomToLoad = currentGameEvent.startRoom;
        else
        {
            roomToLoad = WorldManager.instance.currentRoom;
        }

        if (WorldManager.instance.currentTime != currentGameEvent.timeOfDay ||
            roomToLoad != WorldManager.instance.currentRoom)
        {
            WorldManager.instance.currentRoom = roomToLoad;
            yield return TimeOfDayManager.instance.ChangeTimeOfDay(currentGameEvent.timeOfDay);
            WorldManager.instance.StartLoadingRoom(WorldManager.instance.currentRoom, null);
        }

        else
            currentGameEvent.OnStart();
    }

    public IEnumerator LoadValuesFromSave()
    {
        WorldManager.instance.isLoading = true;
        SaveData data = SaveManager.instance != null ? SaveManager.instance.LoadCurrentSave() : SaveSystem.LoadGame(1);

        GameStateManager.instance.UpdateChapterIndexes(data.chapterIndex, data.chapterSegmentIndex);

        yield return TimeOfDayManager.instance.ChangeTimeOfDay(data.timeOfDay);

        LoadGameEvents(GameStateManager.instance.GetCurrentChapterSegment());

        currentGameEventIndex = data.gameEventIndex;
        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);
        WorldManager.instance.currentRoom = Resources.Load<Room>($"Rooms/{data.currentRoom}");
        MusicManager.instance.PlaySong(Resources.Load<AudioClip>($"Audio/Music/{data.currentMusic}"));
        currentGameEvent.charactersData = data.charactersData.ToDictionary(c => c.key, c => c.value);
        currentGameEvent.objectsData = data.objectsData.ToDictionary(c => c.key, c => c.value);

        CameraManager.instance.player.transform.position =
            new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        ;
        CameraManager.instance.cameraTransform.localPosition =
            new Vector3(data.cameraPosition[0], data.cameraPosition[1], data.cameraPosition[2]);
        CameraManager.instance.cameraTransform.localRotation =
            Quaternion.Euler(new Vector3(data.cameraRotation[0], data.cameraRotation[1], data.cameraRotation[2]));

        VNConversationSegment currentConversation = conversationDatabase.Get(data.currentConversation);
        if (currentConversation != null)
        {
            VNNodePlayer.instance.lineIndex = data.currentLineIndex;
            VNNodePlayer.instance.StartConversation(currentConversation);
            CameraManager.instance.initialRotation = Quaternion.Euler(new Vector3(data.conversationInitialRotation[0],
                data.conversationInitialRotation[1], data.conversationInitialRotation[2]));
        }
        else
        {
            CursorManager.instance.Show();
        }

        WorldManager.instance.Initialize();
    }

    public IEnumerator StartNewVnSegment()
    {
        LoadGameEvents(GameStateManager.instance.chapters[GameStateManager.instance.chapterIndex]
            .chapterSegments[GameStateManager.instance.chapterSegmentIndex]);
        VNUIAnimator.instance.chapterNameText.text =
            GameStateManager.instance.chapters[GameStateManager.instance.chapterIndex].chapterName;
        yield return TimeOfDayManager.instance.ChangeTimeOfDay(gameEvents[0].timeOfDay);
        currentGameEvent = Instantiate(gameEvents[0]);
        if (currentGameEvent.startRoom != null)
            WorldManager.instance.currentRoom = currentGameEvent.startRoom;
        WorldManager.instance.StartLoadingRoom(WorldManager.instance.currentRoom, null);
    }
}