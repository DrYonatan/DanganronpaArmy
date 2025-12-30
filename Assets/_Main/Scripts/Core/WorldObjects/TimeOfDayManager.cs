using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public string GetTimeScene(TimeOfDay timeOfDay)
    {
        string sceneName = timeScenes.Find(x => x.timeOfDay == timeOfDay).scene;
        return sceneName;
    }
}