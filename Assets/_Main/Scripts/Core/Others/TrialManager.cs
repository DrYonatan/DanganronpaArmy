using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    public float maxHP = 5f;
    public float hp;
    public float maxConsentration = 5f;
    public float concentration;

    public void InitializeMeters()
    {
        hp = maxHP;
        concentration = maxConsentration;
    }
}

public class TrialManager : MonoBehaviour
{
    public static TrialManager instance { get; private set; }
    public List<TrialSegment> segments = new List<TrialSegment>();
    private int currentIndex = 0;
    public List<CharacterStand> characterStands;
    public PlayerStats playerStats = new PlayerStats();
    public PlayerBarsAnimator barsAnimator;

    void Awake()
    {
        instance = this;
        playerStats.InitializeMeters();
    }
    void Start()
    {
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
            barsAnimator.IncreaseHealth(amount);
        playerStats.hp += Math.Min(amount, playerStats.maxHP);
    }

    public void DecreaseHealth(float amount)
    {
        playerStats.hp -= amount;
        barsAnimator.DecreaseHealth(amount);
        if (playerStats.hp <= 0)
            GameOver();
    }
    
    void GameOver()
    {
        playerStats.hp = 5f;
        TrialSegment segment = Instantiate(segments[currentIndex]);
        segment.Play();
    }
}
