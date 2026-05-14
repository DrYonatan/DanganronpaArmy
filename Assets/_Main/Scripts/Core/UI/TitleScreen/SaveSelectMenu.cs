using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveSelectMenu : TitleScreenSubMenu
{
    public SaveSlotButton buttonPrefab;
    public RectTransform savesContainer;

    public float buttonHeight = 100f;
    public int visibleCount = 7;

    public SaveSlotButton.Mode mode;

    public GameObject confirmPopup;
    public TitleScreenActionButton confirmButton;
    public TitleScreenActionButton cancelButton;
    public bool onConfirm;
    public bool confirmPopupActive;
    public TextMeshProUGUI confirmText;

    void Awake()
    {
        for (int i = 1; i < SaveManager.instance.saveSlotAmount; i++)
        {
            GenerateSaveSlot(i);
        }

        confirmText.text = mode == SaveSlotButton.Mode.Save ? "האם תרצה לשמור על גבי שמירה זו?" : "האם תרצה לטעון שמירה זו?";
    }

    private void Update()
    {
        if (confirmPopupActive)
        {
            HandleConfirmPopup();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            buttons[currentItemIndex].DisableHover();
            currentItemIndex = (currentItemIndex + 1) % buttons.Count;
            buttons[currentItemIndex].HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(selectionSound);
            UpdateScroll();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            buttons[currentItemIndex].DisableHover();
            currentItemIndex = (currentItemIndex - 1 + buttons.Count) %
                               buttons.Count;

            buttons[currentItemIndex].HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(selectionSound);
            UpdateScroll();
        }
        
        
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !confirmPopupActive)
        {
            bool isSlotValid = ((SaveSlotButton)buttons[currentItemIndex]).CheckValidity();

            if (isSlotValid)
            {
                buttons[currentItemIndex].Click();
                OpenConfirmationPopup(); 
            }
        }
    }

    private void OpenConfirmationPopup()
    {
        confirmPopupActive = true;
        confirmPopup.SetActive(true);
        onConfirm = false;
        cancelButton.HoverButtonAnimation();
    }

    private void CloseConfirmationPopup()
    {
        confirmPopupActive = false;
        confirmPopup.SetActive(false);
        cancelButton.DisableHover();
        confirmButton.DisableHover();
        buttons[currentItemIndex].HoverButtonAnimation();
    }

    private void HandleConfirmPopup()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            cancelButton.DisableHover();
            confirmButton.HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(selectionSound);
            onConfirm = true;
        }

        else if (Input.GetKeyDown(KeyCode.A))
        {
            confirmButton.DisableHover();
            cancelButton.HoverButtonAnimation();
            SoundManager.instance.PlaySoundEffect(selectionSound);
            onConfirm = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (onConfirm)
            {
                LoadOrSave();
            }
            CloseConfirmationPopup();
        }
    }
    
    private void LoadOrSave()
    {
        SaveSlotButton button = (SaveSlotButton)buttons[currentItemIndex];
        if (mode == SaveSlotButton.Mode.Load)
        {
            button.Load();
        }
        else if (mode == SaveSlotButton.Mode.Save)
        {
            button.Save();
        }
    }

    private void GenerateSaveSlot(int index)
    {
        SaveSlotButton button = Instantiate(buttonPrefab, savesContainer);
        button.mode = mode;
        button.slot = index;
        buttons.Add(button);
    }

    private void UpdateScroll()
    {
        savesContainer.DOAnchorPosY(Mathf.Max((currentItemIndex - (visibleCount - 1)) * (buttonHeight + 50), 0), 0.2f)
            .SetUpdate(true);
    }
}