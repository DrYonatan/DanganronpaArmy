using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotItem : MonoBehaviour
{
    public int saveSlot;
    
    public void OnClick()
    {
        SaveManager.instance.SelectSaveSlot(saveSlot);
        SaveData data = SaveManager.instance.LoadCurrentSave();
        SceneManager.LoadScene($"_Main/Scenes/{data.scene}");
    }
}
