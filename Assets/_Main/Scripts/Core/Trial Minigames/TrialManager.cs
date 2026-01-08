using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
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
    public CharacterStand protagonistStand;

    public PlayerStats playerStats = new PlayerStats();
    public PlayerBarsAnimator barsAnimator;
    public RectTransform globalUI;
    public TrialIntro introAnimation;

    public RectTransform failedScreen;
    public Image failedTextImage;
    
    void Awake()
    {
        instance = this;
        if (SaveManager.instance != null)
            LoadValuesFromSave(SaveManager.instance.currentSaveSlot);
    }

    void Start()
    {
        ImageScript.instance.UnFadeToBlack(0.1f);
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
                 amount / 2); // Fill either the amount, or what remains to fill before the meter if already full
        }

        playerStats.hp = Math.Min(playerStats.hp + amount, playerStats.maxHP);
    }

    public void DecreaseHealthDefault(float amount)
    {
        playerStats.hp -= amount;
        barsAnimator.DecreaseHealth(playerStats.hp, 0.5f);
    }

    public void DecreaseHealthFromMeter(Image meter, float amount)
    {
        playerStats.hp -= amount;
        barsAnimator.DecreaseHealthFromMeter(meter, playerStats.hp, 0.5f);
    }


    public IEnumerator GameOver()
    {
        TrialDialogueManager.instance.animator.FaceAppear();
        barsAnimator.globalHealthMeter.fillAmount = 0f;
        yield return TrialDialogueManager.instance.RunNodes(UtilityNodesRuntimeBank.instance.nodesCollection
            .gameOverNodes);
        barsAnimator.HideGlobalBars(0.2f);
        TrialDialogueManager.instance.ConversationEnd();
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

    public void FadeCharactersExcept(Character character, float opacity, float duration)
    {
        foreach (CharacterStand stand in characterStands)
        {
            if (stand.character != character)
            {
                stand.spriteRenderer.DOFade(opacity, duration);
                stand.silhouetteRenderer.DOFade(opacity, duration);
            }
        }
    }

    public IEnumerator ShowFailedScreen()
    {
        yield return new WaitForSeconds(0.5f);
        failedScreen.anchoredPosition = new Vector2(0, 1200);
        Color color = failedTextImage.color;
        color.a = 0f;
        failedTextImage.color = color;
        failedTextImage.rectTransform.localScale = Vector3.one;
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(failedScreen.DOAnchorPosY(0, 0.2f));
        sequence.Append(failedTextImage.DOFade(1f, 0.1f));
        sequence.AppendInterval(1f);
        sequence.Append(failedTextImage.rectTransform.DOScale(2f, 0.2f));
        sequence.Join(failedTextImage.DOFade(0, 0.2f));
        sequence.AppendCallback(() => ImageScript.instance.FadeToBlack(0.2f));
        
        yield return new WaitForSeconds(2.5f);

        failedScreen.anchoredPosition = new Vector2(0, 1200);
    }
}