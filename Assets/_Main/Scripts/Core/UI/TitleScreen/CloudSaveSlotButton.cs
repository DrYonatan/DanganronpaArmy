using System;
using System.Collections.Generic;
using UnityEngine;

public class CloudSaveSlotButton : SaveSlotButton
{
    protected override void UpdateFileText()
    {
        User user = UserDataManager.instance.loggedInUser;
        data = user.saves[slot];
        if (data != null)
        {
            List<Chapter> chapters = Resources.Load<ChaptersBank>("ChaptersBank").chapters;
            slotText.text = $"{slot + 1} - {chapters[data.chapterIndex].chapterName}";
            if (data.saveTime != null)
            {
                DateTime time = DateTime.Parse(data.saveTime);
                string date = time.ToString("dd/MM/yyyy HH:mm");
                dateText.text = date;
            }
        }
        else
        {
            slotText.text = "שמירה ריקה";
            dateText.text = "";
        }
    }

    public override void Save()
    {
        SaveData data = null;
        if (ProgressManager.instance != null)
        {
            data = SaveManager.instance.GetVNSaveData();
        }
        else if (TrialManager.instance != null)
        {
            data = SaveManager.instance.GetVNSaveData();
        }

        UserDataManager.instance.UpdateCloudSave(slot, data);

        UpdateFileText();
    }

    public override void Load()
    {
        SaveManager.instance.isCloud = true;
        SoundManager.instance.PlaySoundEffect(soundEffect);
        SaveManager.instance.SelectCloudSaveSlot(slot);

        if (TitleScreenMainMenu.instance != null)
            TitleScreenMainMenu.instance.GoToGameAnimation(UserDataManager.instance.loggedInUser.saves[slot].scene);
        else
        {
            StartCoroutine(LoadFile(SaveManager.instance.LoadCurrentSave()));
        }
    }
}