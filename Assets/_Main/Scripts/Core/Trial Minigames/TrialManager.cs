using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using DG.Tweening;
using DIALOGUE;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerStats
{
    public float maxHP = 5f;
    public float hp;

    public void InitializeMeters()
    {
        hp = maxHP;
        TrialManager.instance.barsAnimator.SetBarsFillAmount(TrialManager.instance.playerStats.hp, TimeManipulationManager.instance.concentration);
    }
}

public class TrialManager : MonoBehaviour
{
    public static TrialManager instance { get; private set; }
    public List<TrialSegment> segments = new List<TrialSegment>();
    public int currentIndex = 0;
    public List<CharacterStand> characterStands;
    public PlayerStats playerStats = new PlayerStats();
    public PlayerBarsAnimator barsAnimator;
    public RectTransform globalUI;
    public TrialIntro introAnimation;

    void Awake()
    {
        instance = this;
        if (SaveManager.instance != null)
            LoadValuesFromSave(SaveManager.instance.currentSaveSlot);
    }

    void Start()
    {
        if (currentIndex == 0)
        {
            playerStats.InitializeMeters();
            StartCoroutine(StartPipeline());
        }
        else
        {
            TrialSegment segment = Instantiate(segments[currentIndex]);
            segment.Play();
        }
    }

    private IEnumerator StartPipeline()
    {
        TrialIntro intro = Instantiate(introAnimation, globalUI);
        intro.transform.SetAsFirstSibling();
        yield return intro.Animate();
        ImageScript.instance.UnFadeToBlack(0.2f);
        DialogueSystem.instance.dialogueBoxAnimator.Initialize();
        yield return CameraController.instance.FovOutro();
        TrialSegment segment = Instantiate(segments[currentIndex]);
        segment.Play();
    }

    public void OnSegmentFinished()
    {
        currentIndex++;
        TrialSegment segment = Instantiate(segments[currentIndex]);
        segment.Play();
    }

    public void IncreaseHealth(float amount)
    {
        if (playerStats.hp < playerStats.maxHP)
        {
            barsAnimator.IncreaseHealth(Math.Min(amount, playerStats.maxHP - playerStats.hp),
                0.5f); // Fill either the amount, or what remains to fill before the meter if already full
        }

        playerStats.hp = Math.Min(playerStats.hp + amount, playerStats.maxHP);
    }

    public void DecreaseHealthDefault(float amount)
    {
        playerStats.hp -= amount;
        barsAnimator.DecreaseHealth(playerStats.hp, 0.5f);
        if (playerStats.hp <= 0)
            StartCoroutine(GameOver());
    }

    public void DecreaseHealthFromMeter(Image meter, float amount)
    {
        playerStats.hp -= amount;
        barsAnimator.DecreaseHealthFromMeter(meter, playerStats.hp, 0.5f);
        if (playerStats.hp <= 0)
            StartCoroutine(GameOver());
    }


    private IEnumerator GameOver()
    {
        yield return TrialDialogueManager.instance.RunNodes(UtilityNodesRuntimeBank.instance.nodesCollection
            .gameOverNodes);
        playerStats.hp = 5f;
        TrialSegment segment = Instantiate(segments[currentIndex]);
        segment.Play();
    }

    private void LoadValuesFromSave(int slot)
    {
        SaveData data = SaveManager.instance != null
            ? SaveManager.instance.LoadCurrentSave()
            : SaveSystem.LoadGame(slot);

        currentIndex = data.trialSegmentIndex;
        TrialDialogueManager.instance.currentLineIndex = data.currentLineIndex;
        playerStats.hp = data.hp;
    }

    public void FadeCharactersExcept(Character character, float opacity)
    {
        foreach (CharacterStand stand in characterStands)
        {
            if (stand.character != character)
            {
                stand.spriteRenderer.DOFade(opacity, 0.5f);
                stand.silhouetteRenderer.DOFade(opacity, 0.5f);
            }
        }
    }
}