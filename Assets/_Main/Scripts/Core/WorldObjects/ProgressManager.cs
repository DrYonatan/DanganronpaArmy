using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
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

        currentGameEvent.OnStart();
    }

    public IEnumerator LoadValuesFromSave()
    {
        WorldManager.instance.isLoading = true;
        SaveData data = SaveManager.instance != null ? SaveManager.instance.LoadCurrentSave() : SaveSystem.LoadGame(1);

        GameStateManager.instance.UpdateChapterIndexes(data.chapterIndex, data.chapterSegmentIndex);
        VNUIAnimator.instance.chapterNameText.text = GameStateManager.instance.GetCurrentChapter().chapterName;
        
        yield return TimeOfDayManager.instance.ChangeTimeOfDay(data.timeOfDay);

        LoadGameEvents(GameStateManager.instance.GetCurrentChapterSegment());

        currentGameEventIndex = data.gameEventIndex;
        currentGameEvent = Instantiate(gameEvents[currentGameEventIndex]);
        WorldManager.instance.currentRoom = Resources.Load<Room>($"Rooms/{data.currentRoom}");
        MusicManager.instance.PlaySong(Resources.Load<AudioClip>($"Audio/Music/{data.currentMusic}"));
        currentGameEvent.LoadSave(data);
        
        GameStateManager.instance.SetUIState(data.uiState);
        GameStateManager.instance.InitiateUIState();

        CameraManager.instance.player.transform.position =
            new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        ;
        CameraManager.instance.cameraTransform.localPosition =
            new Vector3(data.cameraPosition[0], data.cameraPosition[1], data.cameraPosition[2]);
        CameraManager.instance.cameraTransform.localRotation =
            Quaternion.Euler(new Vector3(data.cameraRotation[0], data.cameraRotation[1], data.cameraRotation[2]));

        if(WorldManager.instance.currentRoom != null)
           WorldManager.instance.Initialize();
    }

    public IEnumerator StartNewVnSegment()
    {
        GameStateManager.instance.InitiateUIState();
        LoadGameEvents(GameStateManager.instance.chapters[GameStateManager.instance.chapterIndex]
            .chapterSegments[GameStateManager.instance.chapterSegmentIndex]);
        VNUIAnimator.instance.chapterNameText.text =
            GameStateManager.instance.chapters[GameStateManager.instance.chapterIndex].chapterName;
        currentGameEvent = Instantiate(gameEvents[0]);
        WorldManager.instance.currentRoom = null;
        yield return TimeOfDayManager.instance.ChangeTimeOfDay(currentGameEvent.timeOfDay);
        currentGameEvent.OnStart();
    }
}