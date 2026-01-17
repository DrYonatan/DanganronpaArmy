using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public string firstScene;

    public int currentSaveSlot;

    public int saveSlotAmount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void StartNewGame()
    {
        currentSaveSlot = -1;
        TitleScreenMainMenu.instance.GoToGameAnimation(firstScene);
    }

    public void SelectSaveSlot(int slot)
    {
        currentSaveSlot = slot;
    }

    public SaveData LoadCurrentSave()
    {
        return SaveSystem.LoadGame(currentSaveSlot);
    }

    public void SaveGameVn(int slot)
    {
        SaveData data = new SaveData(GameStateManager.instance.chapterIndex,
            GameStateManager.instance.chapterSegmentIndex,
            ProgressManager.instance.currentGameEventIndex,
            WorldManager.instance.currentRoom.name,
            VNNodePlayer.instance.currentConversation?.guid, VNNodePlayer.instance.lineIndex,
            MusicManager.instance.audioSource.clip ? MusicManager.instance.audioSource.clip.name : "",
            ProgressManager.instance.currentGameEvent.charactersData,
            ProgressManager.instance.currentGameEvent.objectsData,
            GameStateManager.instance.GetCurrentChapterSegment().GetSceneName(),
            GameStateManager.instance.charactersRanks, CameraManager.instance.player.transform.position,
            CameraManager.instance.cameraTransform.localPosition,
            CameraManager.instance.cameraTransform.localRotation.eulerAngles,
            CameraManager.instance.initialRotation.eulerAngles, WorldManager.instance.currentTime, 0, 0);
        SaveSystem.SaveGame(data, slot);
    }

    public void SaveGameTrial(int slot)
    {
        SaveData data = new SaveData(GameStateManager.instance.chapterIndex,
            GameStateManager.instance.chapterSegmentIndex, TrialManager.instance.currentIndex, "", "",
            TrialDialogueManager.instance.currentLineIndex,
            MusicManager.instance.audioSource.clip ? MusicManager.instance.audioSource.clip.name : "", null, null,
            SceneManager.GetActiveScene().name, GameStateManager.instance.charactersRanks,
            Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, TimeOfDay.Day,
            TrialManager.instance.currentIndex,
            TrialManager.instance.playerStats.hp);

        SaveSystem.SaveGame(data, slot);
    }
}