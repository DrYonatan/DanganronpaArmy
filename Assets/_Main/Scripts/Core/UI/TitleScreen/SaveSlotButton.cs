using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (TitleScreenMainMenu.instance != null)
            TitleScreenMainMenu.instance.GoToGameAnimation(SaveManager.instance.LoadCurrentSave().scene);
        else
        {
            StartCoroutine(LoadFile());
        }
    }

    private IEnumerator LoadFile()
    {
        Time.timeScale = 1f;
        ImageScript.instance.FadeToBlack(0.2f);
        yield return new WaitForSeconds(1f);
        DOTween.KillAll();
        string sceneToLoad = SaveManager.instance.LoadCurrentSave().scene;
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