using System.Linq;
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
        SaveData data = new SaveData(ProgressManager.instance.currentGameEventIndex,
            WorldManager.instance.currentRoom.name,
            VNNodePlayer.instance.currentConversation?.guid, VNNodePlayer.instance.lineIndex,
            MusicManager.instance.audio.name,
            ProgressManager.instance.currentGameEvent.charactersData,
            ProgressManager.instance.currentGameEvent.objectsData, SceneManager.GetActiveScene().name,
            ProgressManager.instance.charactersRanks, CameraManager.instance.cameraTransform.position,
            CameraManager.instance.cameraTransform.rotation.eulerAngles, 0, 0);
        SaveSystem.SaveGame(data, slot);
    }

    public void SaveGameTrial(int slot)
    {
        SaveData prevData = SaveSystem.LoadGame(currentSaveSlot);
        SaveData data = new SaveData(TrialManager.instance.currentIndex, "", "",
            TrialDialogueManager.instance.currentLineIndex,
            MusicManager.instance.audio ? MusicManager.instance.audio.name : "", null, null,
            SceneManager.GetActiveScene().name, prevData.characterRanks.ToDictionary(c => c.key, c => c.value),
            new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), TrialManager.instance.currentIndex,
            TrialManager.instance.playerStats.hp);

        SaveSystem.SaveGame(data, slot);
    }
}