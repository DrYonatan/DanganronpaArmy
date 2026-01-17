using TMPro;
using UnityEngine;

public class SaveSlotButton : TitleScreenMenuButton
{
    public enum Mode
    {
        Save,
        Load
    }

    public int slot;
    public TextMeshProUGUI slotText;
    private SaveData data;
    public Mode mode;
    public AudioClip errorSound;

    void Start()
    {
        UpdateFileText();
    }

    private void UpdateFileText()
    {
        data = SaveSystem.LoadGame(slot);
        if (data != null)
        {
            slotText.text = $"שמירה  {slot}";
        }
        else
        {
            slotText.text = "שמירה ריקה";
        }
    }

    public override void Click()
    {
        if (mode == Mode.Load)
        {
            Load();
        }
        else if (mode == Mode.Save)
        {
            Save();
        }
    }

    private void Save()
    {
        if (ProgressManager.instance != null)
            SaveManager.instance.SaveGameVn(slot);
        else if (TrialManager.instance != null)
            SaveManager.instance.SaveGameTrial(slot);
        
        UpdateFileText();
    }

    private void Load()
    {
        if (data == null)
        {
            SoundManager.instance.PlaySoundEffect(errorSound);
            return;
        }

        SoundManager.instance.PlaySoundEffect(soundEffect);
        SaveManager.instance.SelectSaveSlot(slot);
        TitleScreenMainMenu.instance.GoToGameAnimation(SaveManager.instance.LoadCurrentSave().scene);
    }
}