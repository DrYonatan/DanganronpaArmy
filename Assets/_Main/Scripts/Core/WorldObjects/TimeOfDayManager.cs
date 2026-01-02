using System;
using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TimeOfDay
{
    Dawn,
    Day,
    Night
}

public class TimeOfDayManager : MonoBehaviour
{
    [Serializable]
    public class TimeScene
    {
        public string scene;
        public TimeOfDay timeOfDay;
        public Color mainColor;
    }

    public static TimeOfDayManager instance { get; private set; }

    public List<TimeScene> timeScenes = new();

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void ChangeTimeOfDay(TimeOfDay timeOfDay)
    {
        StartCoroutine(ChangingTime(timeOfDay));
    }

    public IEnumerator ChangingTime(TimeOfDay timeOfDay)
    {
        ImageScript.instance.FadeToBlack(0.2f);
        
        yield return new WaitForSeconds(0.5f);
        
        DialogueSystem.instance.dialogueBoxAnimator.namePlate.GetComponent<Image>().color =
            timeScenes.Find(x => x.timeOfDay == timeOfDay).mainColor;
        WorldManager.instance.currentTime = timeOfDay;
        SceneManager.LoadScene(GetTimeScene(timeOfDay));
        
        yield return new WaitForSeconds(0.1f);
        
        ImageScript.instance.UnFadeToBlack(0.2f);
    }

    private string GetTimeScene(TimeOfDay timeOfDay)
    {
        string sceneName = timeScenes.Find(x => x.timeOfDay == timeOfDay).scene;
        return sceneName;
    }
}