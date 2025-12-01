using System.Collections;
using TMPro;
using UnityEngine;

public class SaveSlotButton : TitleScreenMenuButton
{
    public int slot;
    public TextMeshProUGUI slotText;
    private SaveData data;

    void Start()
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
        if (data == null)
            return;
        SoundManager.instance.PlaySoundEffect(soundEffect);
        SaveManager.instance.SelectSaveSlot(slot);
        TitleScreenMainMenu.instance.GoToGameAnimation(SaveManager.instance.LoadCurrentSave().scene);
    }
    
}
