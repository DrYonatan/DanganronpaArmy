using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public CanvasGroup canvasGroup;
    
    public bool isPlaying = false;
    public static CutSceneManager instance { get; private set; }
    private void Awake()
    {
        instance = this;
        videoPlayer.transform.parent.gameObject.SetActive(false);

    }
    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
    public IEnumerator PlayCutscene(VideoClip clip)
    {
        videoPlayer.transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
        
        videoPlayer.clip = clip;
        videoPlayer.Play();
        canvasGroup.alpha = 1;
        
        yield return new WaitForSeconds((float)clip.length);
    }
}
