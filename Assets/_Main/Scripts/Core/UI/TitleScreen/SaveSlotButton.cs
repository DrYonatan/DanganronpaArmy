using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

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
        image.DOKill();
        TitleScreenMainMenu.instance.KillAllTweens();
        SoundManager.instance.PlaySoundEffect(soundEffect);
        SaveManager.instance.SelectSaveSlot(slot);
        SceneManager.LoadScene(SaveManager.instance.LoadCurrentSave().scene);
    }
}
