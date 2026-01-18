using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DIALOGUE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CharacterInfo 
{
    public string name;
    public Sprite faceIcon;
    public Sprite largeIcon;
    public string birth;
    public string city;
    public string releaseDate;
    public int age;
    public int profile;
    public int dapar;
    public int height;
    public int weight;
    public int progressionLevel;
    public string role;
    public string roleDescription;
}
public class ReportCardMenu: MenuScreen
{
    private int currentCharacterIndex;
    public AudioClip moveSelectionSound;
    public List<CharacterIcon> charactersListUI;
    public List<CharacterInfo> characterInfoList;
    public CharacterIcon characterIconPrefab;
    public Transform charactersBarContainer;
    public Image characterBigIcon;
    public TextMeshProUGUI name;
    public TextMeshProUGUI birth;
    public TextMeshProUGUI age;
    public TextMeshProUGUI city;
    public TextMeshProUGUI releaseDate;
    public TextMeshProUGUI profile;
    public TextMeshProUGUI dapar;
    public TextMeshProUGUI height;
    public TextMeshProUGUI weight;
    public TextMeshProUGUI progressionLevel;
    public TextMeshProUGUI role;
    public TextMeshProUGUI roleDescription;
    public TextMeshProUGUI socialStatus;
    public List<RectTransform> blocks;
    private List<float> originalPosYs = new List<float>();
    public GameObject normalContent;
    public GameObject protagonistContent;
    private const string PROTAGONIST_NAME = "אלון";
    public List<string> statuses = new List<string>();
    string GetSocialStatus(int totalProgress)
    {
        int index = 0;
        
        if (totalProgress < 2)
        {
            index = 0;
        } 
        else if (totalProgress < 4)
        {
            index = 1;
        }
        else if (totalProgress < 7)
        {
            index = 2;
        }
        else if (totalProgress < 10)
        {
            index = 3;
        }
        else if (totalProgress < 15)
        {
            index = 4;
        }
        else
        {
            index = 5;
        }

        return statuses[index];
    }
    void Awake()
    {
        foreach (CharacterInfo characterInfo in characterInfoList)
        {
            AddCharacterToList(characterInfo);
        }
        
        socialStatus.text = GetSocialStatus(characterInfoList.Sum((characterInfo) => characterInfo.progressionLevel));

        foreach (RectTransform block in blocks)
        {
            originalPosYs.Add(block.anchoredPosition.y);
        }
    }
    void AddCharacterToList(CharacterInfo characterInfo)
    {
        CharacterIcon instantiated = Instantiate(characterIconPrefab);
        instantiated.transform.SetParent(charactersBarContainer, false);
        instantiated.SetIcon(characterInfo.faceIcon);
        charactersListUI.Add(instantiated); 
    }
    public override void Open()
    {
        base.Open();
        currentCharacterIndex = 0;
        UpdateUI();
    } 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characterInfoList.Count;
            UpdateUI();
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentCharacterIndex = (currentCharacterIndex - 1 + characterInfoList.Count) %
                                    characterInfoList.Count;
            UpdateUI();
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerInputManager.instance.pauseMenu.GoBackToGeneral();
        }
    }

    void UpdateUI()
    {
        CharacterInfo currentCharacterInfo = characterInfoList[currentCharacterIndex];
        if (currentCharacterInfo != null)
        {
            characterBigIcon.sprite = currentCharacterInfo.largeIcon;
            name.text = currentCharacterInfo.name;
            birth.text = currentCharacterInfo.birth;
            age.text = $"({currentCharacterInfo.age})";
            city.text = currentCharacterInfo.city;
            releaseDate.text = currentCharacterInfo.releaseDate;
            profile.text = currentCharacterInfo.profile.ToString();
            dapar.text = currentCharacterInfo.dapar.ToString();
            height.text = $"{currentCharacterInfo.height} (cm)";
            weight.text = $"{currentCharacterInfo.weight} (kg)";
            role.text = currentCharacterInfo.role;
            roleDescription.text = currentCharacterInfo.roleDescription;

            if (currentCharacterInfo.name == PROTAGONIST_NAME)
            {
                normalContent.SetActive(false);
                protagonistContent.SetActive(true);
            }
            else
            {
                progressionLevel.text = currentCharacterInfo.progressionLevel.ToString();
                protagonistContent.SetActive(false);
                normalContent.SetActive(true);
            }
            
            foreach (CharacterIcon item in charactersListUI)
            {
                item.StopHover();
            }

            if (charactersListUI.Count > 0)
                charactersListUI[currentCharacterIndex].StartHover();

            BlocksIntro();
        }
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        UpdateUI();
    }

    void BlocksIntro()
    {
        int i = 0;
        
        foreach (RectTransform block in blocks)
        {
            block.DOKill();
            CanvasGroup canvasGroup = block.GetComponent<CanvasGroup>();
            canvasGroup.DOKill();
            block.anchoredPosition = new UnityEngine.Vector2(block.anchoredPosition.x, originalPosYs[i]) + UnityEngine.Vector2.down * 100;
            block.DOAnchorPosY(originalPosYs[i], 0.3f).SetDelay(i * 0.05f).SetUpdate(true);
            canvasGroup.alpha = 0.3f;
            canvasGroup.DOFade(1f, 0.6f).SetUpdate(true);
            i++;
        }
    }
    
}