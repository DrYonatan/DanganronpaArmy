using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotButton : TitleScreenMenuButton
{
    public enum Mode
    {
        Save,
        Load
    }

    public int slot;
    public TextMeshProUGUI slotText;
    public TextMeshProUGUI dateText;
    protected SaveData data;
    public Mode mode;
    public AudioClip errorSound;

    void Start()
    {
        UpdateFileText();
    }

    protected virtual void UpdateFileText()
    {
        data = SaveSystem.LoadGame(slot);
        if (data != null)
        {
            List<Chapter> chapters = Resources.Load<ChaptersBank>("ChaptersBank").chapters;
            slotText.text = $"{slot} - {chapters[data.chapterIndex].chapterName}";
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
    
    public virtual void Save()
    {
        if (ProgressManager.instance != null)
            SaveManager.instance.SaveGameVn(slot);
        else if (TrialManager.instance != null)
            SaveManager.instance.SaveGameTrial(slot);

        UpdateFileText();
    }

    public bool CheckValidity()
    {
        if (mode == Mode.Load)
        {
            if (data == null || data.saveTime == null || data.saveTime == "")
            {
                SoundManager.instance.PlaySoundEffect(errorSound);
                return false;
            }  
        }

        return true;
    }

    public virtual void Load()
    {
        SaveManager.instance.isCloud = false;
        SoundManager.instance.PlaySoundEffect(soundEffect);
        SaveManager.instance.SelectSaveSlot(slot);

        if (TitleScreenMainMenu.instance != null)
            TitleScreenMainMenu.instance.GoToGameAnimation(SaveManager.instance.LoadCurrentSave().scene);
        else
        {
            StartCoroutine(LoadFile(SaveManager.instance.LoadCurrentSave()));
        }
    }

    protected IEnumerator LoadFile(SaveData currentSave)
    {
        Time.timeScale = 1f;
        ImageScript.instance.FadeToBlack(0.2f);
        yield return new WaitForSeconds(1f);
        DOTween.KillAll();
        string sceneToLoad = currentSave.scene;
        SceneManager.LoadScene(sceneToLoad);

        Camera sceneTransitionCam = GameStateManager.instance.sceneTransitionCamera;
        sceneTransitionCam.gameObject.SetActive(true);
        
        sceneTransitionCam.transform.SetParent(GameStateManager.instance.transform.parent);
        if (GameStateManager.instance.persistentObject != null)
            Destroy(GameStateManager.instance.persistentObject);
        Destroy(GameStateManager.instance.gameObject);
        yield return new WaitForSeconds(0.5f);
    }
}