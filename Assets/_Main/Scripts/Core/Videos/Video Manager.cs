using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;


public class VideoManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private GameObject dialogueBox;
    private GameObject nameText;
    private GameObject ultimateText;
    public bool isPlaying = false;
    public static VideoManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
        dialogueBox = GameObject.Find("VN controller/Root/Canvas - Overlay/4 - Dialogue");
        nameText = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/5 - Foreground/NameText");
        ultimateText = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/5 - Foreground/UltimateText");
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
        VideoPlayer video = gameObject.GetComponent<VideoPlayer>();
        video.clip = (VideoClip)Resources.Load($"Videos/{cutsceneName}");
        videoPlayer.Play();
        StartCoroutine(Play());
        dialogueBox.transform.localScale = new Vector3(0, 0, 0);
    }

    public void PlayUltimateVideo(string characterName)
    {
        VideoPlayer video = gameObject.GetComponent<VideoPlayer>();
        video.SetDirectAudioMute(0, false);
        dialogueBox.transform.localScale = new Vector3(0, 0, 0);
        videoPlayer.Stop();
        videoPlayer.Play();
        StartCoroutine(Play());
        StartCoroutine(MakeTextAppearOrDisappear(1, true));
        string[] nameAndText = MakeUltimateAndNameText(characterName);
        nameText.GetComponent<TextMeshProUGUI>().SetText(nameAndText[0]);
        ultimateText.GetComponent<TextMeshProUGUI>().SetText(nameAndText[1]);
        StartCoroutine(MakeTextAppearOrDisappear(4.5f, false));

        StartCoroutine(WaitAndThenMoveCharacter(0.31f, characterName, "left"));
        StartCoroutine(WaitAndThenMoveCharacter(4.5f, characterName, "right"));

    }


    public void SetUltimateVideo(string characterName)
    {
        MakeActive();
        gameObject.SetActive(true);
        VideoPlayer video = gameObject.GetComponent<VideoPlayer>();
        video.clip = (VideoClip)Resources.Load($"Videos/UltimateVideo{characterName}");
        video.SetDirectAudioMute(0, true);
        videoPlayer.Play();
    }

    private string[] MakeUltimateAndNameText(string characterName)
    {
        string[] res = new string[2];
        switch (characterName.ToLower())
        {
            case "koby":
                res[0] = "קובי שוורץ";
                res[1] = "רע''צ משטרה צבאית";
                break;

            case "noya":
                res[0] = "נויה פישמן";
                res[1] = "רע''צית מודיעין";
                break;

            case "noa":
                res[0] = "נעה ליבוביץ'";
                res[1] = "רע''צית ת''ש";
                break;

            case "inbal":
                res[0] = "ענבל אשכנזי";
                res[1] = "רע''צית אוויר";
                break;

            case "guy":
                res[0] = "גיא רופין";
                res[1] = "רע''צ תקשוב";
                break;

            case "ariel":
                res[0] = "אריאל קפלינסקי";
                res[1] = "רע''צ גבעתי";
                break;

            case "kfir":
                res[0] = "כפיר כהן";
                res[1] = "רע''צ גולני";
                break;

            case "liel":
                res[0] = "ליאל אברג'יל";
                res[1] = "רע''צית חימוש";
                break;

            case "shiraz":
                res[0] = "שירז רן";
                res[1] = "רע''צית  טיק טוק";
                break;

            case "omer":
                res[0] = "עומר הרוש";
                res[1] = "רע''צ שריון";
                break;

            case "romi":
                res[0] = "רומי הכט";
                res[1] = "רע''צית דין";
                break;

            case "maya":
                res[0] = "מאיה וקנין";
                res[1] = "רע''צית תצפיתנית";
                break;

            case "ohav":
                res[0] = "אוהב בן עזרא";
                res[1] = "רע''צ מטבח";
                break;

            case "roey":
                res[0] = "רועי הדר";
                res[1] = "רע''צ ים";
                break;
        }

        return res;
    }

    IEnumerator WaitAndThenMoveCharacter(float duration, string characterName, string direction)
    {
        yield return new WaitForSeconds(duration);
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
        dialogueBox.transform.localScale = new Vector3(1, 1, 1);
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
        Vector3 addPos = new Vector3(direction == "left" ? -550 : 550, 0, 0);
        Animator anim = character.GetComponent<Animator>();

        Vector3 startPos = character.transform.position;
        Vector3 targetPos = startPos + addPos; // Adjust this vector to change the direction

        while (elapsedTime < duration)
        {
            character.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        character.transform.position = targetPos;
    }

    

}
