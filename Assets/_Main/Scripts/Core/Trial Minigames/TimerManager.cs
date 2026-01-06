using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance { get; private set; }

    public float timer;
    public bool countTime;

    void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }
    
    void Update()
    {
        if(countTime)
            timer -= Time.deltaTime;
    }

    public void SetTimer(float time)
    {
        timer = time;
        countTime = true;
    }

    public void StopTimer()
    {
        countTime = false;
    }

    public void ResumeTimer()
    {
        countTime = true;
    }

    public string GetTimeFormat()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        return timeSpan.ToString(@"mm\:ss\:ffff");
    }
}
