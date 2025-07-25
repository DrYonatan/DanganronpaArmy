﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;
using DIALOGUE;


public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public CanvasGroup dialogueBoxCanvas;
    public GameObject nameText;
    public GameObject ultimateText;
    public bool isPlaying = false;
    public Transform characterSilhouette;
    public static VideoManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
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
        VideoPlayer video = gameObject.GetComponent<VideoPlayer>();
        video.clip = (VideoClip)Resources.Load($"Videos/{cutsceneName}");
        videoPlayer.Play();
        StartCoroutine(Play());
        dialogueBoxCanvas.alpha = 1;
    }

    public void PlayUltimateVideo(string characterName)
    {
        GameObject silhouette =
            GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [{characterName}]/Character - [{characterName}](Clone)");
        silhouette.GetComponent<CanvasGroup>().alpha = 1f;
        GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/1 - Background/UltimateVideo/Screen").GetComponent<CanvasGroup>().alpha = 1;
        VideoPlayer video = gameObject.GetComponent<VideoPlayer>();
        video.SetDirectAudioMute(0, false);
        dialogueBoxCanvas.alpha = 0;
        videoPlayer.Stop();
        videoPlayer.Play();
        StartCoroutine(Play());
        StartCoroutine(MakeTextAppearOrDisappear(1, true));
        string[] nameAndText = MakeUltimateAndNameText(characterName);
        nameText.GetComponent<TextMeshProUGUI>().SetText(nameAndText[0]);
        ultimateText.GetComponent<TextMeshProUGUI>().SetText(nameAndText[1]);
        StartCoroutine(MakeTextAppearOrDisappear(4.5f, false));
        Transform renderer = GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [{characterName}]/Character - [{characterName}](Clone)/Anim/Renderers").transform;
        StartCoroutine(WaitAndThenMoveCharacter(0.31f, characterName, "left", renderer, 4.5f, true));
        
        StartCoroutine(WaitAndThenMoveCharacter(4.5f, characterName, "right", renderer, 0.3f, false));
    }

    IEnumerator SlideSilhouette(Transform silhouette, float duration, bool starts)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = silhouette.localPosition;
        Vector3 targetPosition = startPosition + (starts ? 1 : -1) * (55 * Vector3.up + 65 * Vector3.left);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            silhouette.localPosition = Vector3.Lerp(startPosition,
                targetPosition, elapsedTime / duration);
            yield return null;
        }
        
        silhouette.localPosition = targetPosition;
        if (!starts)
        {
            silhouette.parent.parent.GetComponent<CanvasGroup>().alpha = 0;
        } 
    }

    public void SetUltimateVideo(string characterName)
    {
        GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/1 - Background/UltimateVideo/Screen").GetComponent<CanvasGroup>().alpha = 0;
        MakeActive();
        gameObject.SetActive(true);
        VideoPlayer video = gameObject.GetComponent<VideoPlayer>();
        video.clip = (VideoClip)Resources.Load($"Videos/UltimateVideo{characterName}");
        video.SetDirectAudioMute(0, true);
        videoPlayer.Play();
        GameObject character = GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [{characterName}]");
        GameObject silhouette = Instantiate(character, character.transform);
        silhouette.transform.localScale = Vector3.one;
        silhouette.transform.localPosition = Vector3.zero;
        silhouette.transform.SetSiblingIndex(0);
        silhouette.GetComponentInChildren<Image>().color = Color.black;
        silhouette.GetComponent<CanvasGroup>().alpha = 0;
    }

    private string[] MakeUltimateAndNameText(string characterName)
    {
        string[] res = new string[2];
        switch (characterName.ToLower())
        {
            case "koby":
                res[0] = "קובי  שוורץ";
                res[1] = "רע''צ משטרה צבאית";
                break;

            case "noya":
                res[0] = "נויה  פישמן";
                res[1] = "רע''צית מודיעין";
                break;

            case "noa":
                res[0] = "נעה  ליבוביץ'";
                res[1] = "רע''צית ת''ש";
                break;

            case "inbal":
                res[0] = "ענבל  אשכנזי";
                res[1] = "רע''צית אוויר";
                break;

            case "guy":
                res[0] = "גיא  רופין";
                res[1] = "רע''צ תקשוב";
                break;

            case "ariel":
                res[0] = "אריאל קפלינסקי";
                res[1] = "רע''צ גבעתי";
                break;

            case "kfir":
                res[0] = "כפיר  כהן";
                res[1] = "רע''צ גולני";
                break;

            case "liel":
                res[0] = "ליאל  אברג'יל";
                res[1] = "רע''צית חימוש";
                break;

            case "shiraz":
                res[0] = "שירז  רן";
                res[1] = "רע''צית  טיק טוק";
                break;

            case "omer":
                res[0] = "עומר  הרוש";
                res[1] = "רע''צ שריון";
                break;

            case "romi":
                res[0] = "רומי  הכט";
                res[1] = "רע''צית דין";
                break;

            case "maya":
                res[0] = "מאיה  וקנין";
                res[1] = "רע''צית תצפיתנית";
                break;

            case "ohav":
                res[0] = "אוהב  בן  עזרא";
                res[1] = "רע''צ מטבח";
                break;

            case "roey":
                res[0] = "רועי  הדר";
                res[1] = "רע''צ ים";
                break;
        }

        return res;
    }

    IEnumerator WaitAndThenMoveCharacter(float duration, string characterName,
        string direction, Transform silhouette, float silhouetteDuration, bool starts)
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(SlideSilhouette(silhouette, silhouetteDuration, starts));
        StartCoroutine(MoveCharacter(direction, characterName));
        
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
        videoPlayer.transform.parent.gameObject.SetActive(false);
        DialogueSystem.instance.OnUserPrompt_Next();
        dialogueBoxCanvas.alpha = 1;
        isPlaying = false;
    }

    IEnumerator MakeTextAppearOrDisappear(float duration, bool nameAndUltimateActive)
    {
        float elapsedTime = 0;


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;

        }
        nameText.SetActive(nameAndUltimateActive);
        ultimateText.SetActive(nameAndUltimateActive);

    }

    private static IEnumerator MoveCharacter(string direction, string characterName)
    {

        float elapsedTime = 0;
        float duration = 0.25f;

        GameObject character = GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters");
        Vector3 addPos = new Vector3(direction == "left" ? -650 : 650, 0, 0);
        Animator anim = character.GetComponent<Animator>();

        Vector3 startPos = character.transform.localPosition;
        Vector3 targetPos = startPos + addPos; // Adjust this vector to change the direction

        while (elapsedTime < duration)
        {
            character.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        character.transform.localPosition = targetPos;
    }

    

}
