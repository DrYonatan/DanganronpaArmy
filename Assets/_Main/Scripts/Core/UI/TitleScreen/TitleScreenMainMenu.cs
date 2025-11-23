using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMainMenu : MenuScreen
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveManager.instance.SelectSaveSlot(1);
            SaveData data = SaveManager.instance.LoadCurrentSave();
            SceneManager.LoadScene($"_Main/Scenes/{data.scene}");        }
    }
}
