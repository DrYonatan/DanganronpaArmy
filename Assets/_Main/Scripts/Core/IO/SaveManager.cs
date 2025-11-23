using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public int currentSaveSlot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void SelectSaveSlot(int slot)
    {
        currentSaveSlot = slot;
    }

    public SaveData LoadCurrentSave()
    {
        return SaveSystem.LoadGame(currentSaveSlot);
    }

    public void SaveGame(int slot)
    {
        SaveData data = new SaveData(ProgressManager.instance.currentGameEventIndex,
            WorldManager.instance.currentRoom.name,
            VNNodePlayer.instance.currentConversation?.guid, VNNodePlayer.instance.lineIndex,
            MusicManager.instance.audio.name,
            ProgressManager.instance.currentGameEvent.charactersData,
            ProgressManager.instance.currentGameEvent.objectsData, SceneManager.GetActiveScene().name,
            ProgressManager.instance.charactersRanks, CameraManager.instance.cameraTransform.position,
            CameraManager.instance.cameraTransform.rotation.eulerAngles);
        SaveSystem.SaveGame(data, slot);
    }
}