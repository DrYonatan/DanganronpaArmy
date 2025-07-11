using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public CanvasGroup dialogueBoxCanvas;
    public bool isPlaying = false;
    public static CutSceneManager instance { get; private set; }
    private void Awake()
    {
        instance = this;
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.transform.parent.gameObject.SetActive(false);

    }

    public void MakeActive()
    {
        videoPlayer.transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void PlayCutscene(string cutsceneName)
    {
        gameObject.SetActive(true);
        gameObject.SetActive(true);
        videoPlayer.Stop();
        VideoPlayer video = gameObject.GetComponent<VideoPlayer>();
        video.clip = (VideoClip)Resources.Load($"Videos/{cutsceneName}");
        videoPlayer.Play();
        StartCoroutine(Play());
        dialogueBoxCanvas.alpha = 0;
    }
    public void PlayCutsceneWithoutHiding(string cutsceneName)
    {
        gameObject.SetActive(true);
        gameObject.SetActive(true);
        videoPlayer.Stop();
        VideoPlayer video = gameObject.GetComponent<VideoPlayer>();
        video.clip = (VideoClip)Resources.Load($"Videos/{cutsceneName}");
        videoPlayer.Play();
        StartCoroutine(PlayWithoutHiding());
        dialogueBoxCanvas.alpha = 0;
    }

    public void Hide()
    {
        gameObject.GetComponentInParent<CanvasGroup>().alpha = 0;
    }
    IEnumerator Play()
    {

        float elapsedTime = 0;
        float duration = (float)GetComponent<VideoPlayer>().clip.length;
        isPlaying = true;
        int i = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            if (i == 15)
                gameObject.GetComponentInParent<CanvasGroup>().alpha = 1;
            i++;
            yield return null;

        }
        gameObject.GetComponentInParent<CanvasGroup>().alpha = 0;
        dialogueBoxCanvas.alpha = 1;
        isPlaying = false;
    }
    IEnumerator PlayWithoutHiding()
    {

        float elapsedTime = 0;
        float duration = (float)GetComponent<VideoPlayer>().clip.length;
        isPlaying = true;
        int i = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            if (i == 15)
                gameObject.GetComponentInParent<CanvasGroup>().alpha = 1;
            i++;
            yield return null;

        }
        dialogueBoxCanvas.alpha = 1;
        isPlaying = false;
    }
}
