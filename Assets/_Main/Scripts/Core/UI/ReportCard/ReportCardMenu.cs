using System;
using System.Collections.Generic;
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
    
    void Start()
    {
        foreach (CharacterInfo characterInfo in characterInfoList)
        {
            AddCharacterToList(characterInfo);
        }
        
        UpdateUI();
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
            currentCharacterIndex = (currentCharacterIndex + 1) % EvidenceManager.instance.evidenceList.Count;
            UpdateUI();
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentCharacterIndex = (currentCharacterIndex - 1 + EvidenceManager.instance.evidenceList.Count) %
                                    EvidenceManager.instance.evidenceList.Count;
            UpdateUI();
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
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
            progressionLevel.text = currentCharacterInfo.progressionLevel.ToString();
            role.text = currentCharacterInfo.role;
            roleDescription.text = currentCharacterInfo.roleDescription;
            
            foreach (CharacterIcon item in charactersListUI)
            {
                item.StopHover();
            }

            if (charactersListUI.Count > 0)
                charactersListUI[currentCharacterIndex].StartHover();
        }
    }
    
}