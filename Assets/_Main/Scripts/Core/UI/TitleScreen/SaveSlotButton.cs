using DG.Tweening;
using TMPro;
using UnityEngine;
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
            slotText.text = $"Save #{slot}";
        }
        else
        {
            slotText.text = "Empty";
        }
    }
    
    public override void Click()
    {
        if (data == null)
            return;
        image.DOKill();
        SaveManager.instance.SelectSaveSlot(slot);
        SceneManager.LoadScene(SaveManager.instance.LoadCurrentSave().scene);
    }
}
