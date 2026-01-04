using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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
        public string timeOfDayName;
        public Sprite[] iconSprites = new Sprite[3];
    }

    public static TimeOfDayManager instance { get; private set; }

    public List<TimeScene> timeScenes = new();

    public List<Image> timeColorCodedImages = new();
    public TimeScene currentTimeScene;

    private Sequence iconSeq;
    
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public IEnumerator ChangeTimeOfDay(TimeOfDay timeOfDay)
    {
        CursorManager.instance.Hide();
        ImageScript.instance.FadeToBlack(0.5f);
        VNUIAnimator.instance.Disappear();
        
        yield return new WaitForSeconds(1f);
        
        currentTimeScene = timeScenes.Find(x => x.timeOfDay == timeOfDay);
        
        UpdateUIAccordingToTime(currentTimeScene);
        
        WorldManager.instance.currentTime = timeOfDay;
        SceneManager.LoadScene(GetTimeScene(timeOfDay));
        
        yield return new WaitForSeconds(0.1f);
        
        VNUIAnimator.instance.Appear();
        AnimateIcon();
    }

    private void UpdateUIAccordingToTime(TimeScene timeScene)
    {
        foreach (Image image in timeColorCodedImages)
        {
            image.color = timeScene.mainColor;
        }

        VNUIAnimator.instance.timeText.text = timeScene.timeOfDayName;
    }

    private string GetTimeScene(TimeOfDay timeOfDay)
    {
        string sceneName = timeScenes.Find(x => x.timeOfDay == timeOfDay).scene;
        return sceneName;
    }

    private void AnimateIcon()
    {
        Image timeIcon = VNUIAnimator.instance.timeIcon;
        iconSeq.Kill();
        
        iconSeq = DOTween.Sequence();
        iconSeq.AppendCallback(() => timeIcon.sprite = currentTimeScene.iconSprites[0]);
        iconSeq.AppendInterval(0.4f);
        iconSeq.AppendCallback(() => timeIcon.sprite = currentTimeScene.iconSprites[1]);
        iconSeq.AppendInterval(0.4f);
        iconSeq.AppendCallback(() => timeIcon.sprite = currentTimeScene.iconSprites[2]);
        iconSeq.AppendInterval(0.4f);
        iconSeq.SetLoops(-1);
        iconSeq.SetLink(timeIcon.gameObject);
    }
    
}