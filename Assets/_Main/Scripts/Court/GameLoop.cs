using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using _Main.Scripts.Court;
using Text = TMPro.TextMeshProUGUI;

public class GameLoop : MonoBehaviour
{
    public class TextLine
    {
        public GameObject textGO;
        public RectTransform textRT;
        public List<TextEffect> textEffect;
        public float ttl;
        public TextMeshPro textMeshPro;
        public int correctTMPIndex, correctCharacterIndexBegin, correctCharacterIndexEnd;

        public TextLine(GameObject textGO, RectTransform textRT, List<TextEffect> textEffect, float ttl,
            TextMeshPro tmp, int correctTMPIndex, int correctCharacterIndexBegin, int correctCharacterIndexEnd)
        {
            this.textMeshPro = tmp;
            this.textGO = textGO;
            this.textRT = textRT;
            this.textEffect = textEffect;
            this.ttl = ttl;
            this.correctTMPIndex = correctTMPIndex;
            this.correctCharacterIndexBegin = correctCharacterIndexBegin;
            this.correctCharacterIndexEnd = correctCharacterIndexEnd;
        }

        internal void Apply()
        {
            for (int i = 0; i < textEffect.Count; i++)
            {
                textEffect[i].Apply(textRT);
            }
        }
    }


    public static GameLoop instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] CameraController cameraController;
    [SerializeField] List<CharacterStand> characterStands;
    [SerializeField] Stage stage;
    [SerializeField] Transform textPivot;
    [SerializeField] GameObject textPrefab;
    [SerializeField] CameraEffectController effectController;
    [SerializeField] EvidenceManager evidenceManager;
    [SerializeField] MusicManager musicManager;
    [SerializeField] Text timerText;
    [SerializeField] GameObject shatterTransform;
    public GameObject noThatsWrong;

    float timer;
    int textIndex;

    List<TextLine> textLines;
    CharacterStand characterStand;
    Evidence correctEvidence;

    bool pause;
    public bool finished;
    float stageTimer;
    float defaultStageTime = 600f;

    public GameObject textBulletPrefab;
    public float shootForce = 10f;
    public Transform shootOrigin;
    public Transform textStartPosition;
    public Camera statementsCamera;
    public DebateText currentAimedText;
    public Camera renderTextureCamera;

    // Start is called before the first frame update
    void Start()
    {
        textLines = new List<TextLine>();
        evidenceManager.ShowEvidence(stage.evidences);
        MusicManager.instance.PlaySong(stage.audioClip.name);
        stageTimer = defaultStageTime;
        finished = true;
        StartCoroutine(StartDebate());
    }

    IEnumerator StartDebate()
    { 
        ImageScript.instance.blackFade.GetComponent<CanvasGroup>().alpha = 1f;
        yield return 0;
        ImageScript.instance.UnFadeToBlack(1f);
       yield return StartCoroutine(cameraController.DebateStartCameraMovement(4f));
       finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetPause(!pause);
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 4f;
            }
            else
            {
                Time.timeScale = 1f;
            }

            if (pause == true || finished == true)
            {
                return;
            }

            if (stage.dialogueNodes.Count <= textIndex)
            {
                textIndex = 0;
            }


            timer += Time.deltaTime;
            stageTimer -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(stageTimer);
            timerText.text = timeSpan.ToString(@"mm\:ss\:ffff");

            if (stageTimer < 0f)
            {
                GameOver();
            }
        
            if (textLines.Count == 0)
            {
                StartCoroutine(StartNewNode(textIndex));
                timer = 0;
                textIndex++;
            }

            int index = 0;
            while (index < textLines.Count)
            {
                if (textLines[index].ttl < timer)
                {
                    StartCoroutine(DestroyText(textLines[index].textGO));
                    Destroy(textLines[index].textGO);
                    textLines.RemoveAt(index);
                    index--;
                }

                index++;
            }

            if (textLines.Count > 0)
            {
                for (int i = 0; i < textLines.Count; i++)
                {
                    textLines[i].Apply();
                }
            }

            HandleMouseControl();
        }
    }

    

    private void GameOver()
    {
        finished = true;
    }

    void HandleMouseControl()
    {
        CursorManager.instance.ReticleAsCursor();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        if (Input.GetMouseButtonDown(0))
        {
            ShootText();
        }
    }

    void SetPause(bool _pause)
    {
        pause = _pause;
        if (pause == true)
        {
            MusicManager.instance.LowerVolume();
        }
        else
        {
            MusicManager.instance.RaiseVolume();
        }
    }

    void ShootText()
    {
        Ray ray = statementsCamera.ScreenPointToRay(Input.mousePosition);

        // Create a plane in front of the firePoint (facing the same way as the camera)
        Plane plane = new Plane(statementsCamera.transform.forward,
            shootOrigin.position + statementsCamera.transform.forward * 4f);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - shootOrigin.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction, statementsCamera.transform.up) *
                                  Quaternion.Euler(0, 90f, 0);
            GameObject bullet = Instantiate(textBulletPrefab, shootOrigin.position, rotation);
            
            StartCoroutine(MoveBullet(bullet, direction, 2f));
        }
    }

    IEnumerator MoveBullet(GameObject bullet, Vector3 direction, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            bullet.transform.position += direction * shootForce * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(bullet);
    }


    public void Hit(Vector3 point)
    {
        if (evidenceManager.Check(correctEvidence))
        {
            CorrectChoice();
            gameObject.GetComponent<TextShatterEffect>().Explosion(point);
        }
    }

    private void CorrectChoice()
    {
        finished = true;
        foreach (TextLine text in textLines)
        {
            gameObject.GetComponent<TextShatterEffect>().Shatter(text);
        }

        OnHitStatement();
        CursorManager.instance.Hide();
    }

    public void OnHitStatement()
    {
        StartCoroutine(DebateHitEffect());
    }

    IEnumerator DebateHitEffect()
    {
        renderTextureCamera.gameObject.SetActive(true);
        cameraController.camera.targetTexture = 
            Resources.Load<RenderTexture>("Models/Materials/OtherMaterials/ScreenShatterTexture");
        effectController.Reset();
        Vector3 firstTargetPosition = new Vector3(1f, 3f, -8f);
        Vector3 secondTargetPosition = firstTargetPosition - new Vector3(0f, 0f, 8f);
        StartCoroutine(PlayNoThatsWrong(1.5f));
        StartCoroutine(cameraController.ChangeFov(cameraController.camera.fieldOfView, 8, 0.7f));
        yield return cameraController.MoveCameraOnXAndZ(firstTargetPosition, Quaternion.Euler(0f, -5f, 0f), 0.4f);
        StartCoroutine(cameraController.MoveCameraOnXAndZ(secondTargetPosition, Quaternion.Euler(0f, 0f, 30f), 4f));
        yield return new WaitForSeconds(3f);
        cameraController.camera.targetTexture = null;
        shatterTransform.SetActive(true);
        renderTextureCamera.gameObject.SetActive(false);
    }
    
    IEnumerator PlayNoThatsWrong(float delay)
    {
        yield return new WaitForSeconds(delay);

        noThatsWrong.SetActive(true);
        SoundManager.instance.PlaySoundEffect("nothatswrong");

        yield return new WaitForSeconds(3f);

        noThatsWrong.SetActive(false);
    }

    IEnumerator StartNewNode(int dialogueNodeIndex)
    {
        DebateNode nextNode = stage.dialogueNodes[dialogueNodeIndex];
        SpawnText(nextNode);
        effectController.Reset();
        yield return cameraController.SpinToTarget(characterStand.transform, characterStand.heightPivot, nextNode.positionOffset, nextNode.rotationOffset, nextNode.fovOffset);
       
        foreach (CameraEffect cameraEffect in nextNode.cameraEffects)
        { ;
            effectController.StartEffect(cameraEffect);
        }
    }

    void SpawnText(DebateNode nextNode)
    {
        int correctTMPIndex = -1;
        int correctCharacterIndexBegin = -1;
        int correctCharacterIndexEnd = -1;

        correctEvidence = nextNode.evidence;

        if (nextNode.character != null)
        {
            characterStand = characterStands.Find(stand => stand.character == nextNode.character);
        }

        if (characterStand != null)
        {
            characterStand.state = nextNode.expression;
            characterStand.SetSprite();
        }

        for (int i = 0; i < nextNode.textLines.Count; i++)
        {
            correctTMPIndex = -1;
            correctCharacterIndexBegin = -1;
            correctCharacterIndexEnd = -1;

            GameObject go = Instantiate(textPrefab, statementsCamera.transform.parent);
            go.transform.position = textStartPosition.position;
            go.transform.position += nextNode.textLines[i].spawnOffset;
            go.transform.localScale = nextNode.textLines[i].scale;

            TextMeshPro tmp = go.GetComponent<TextMeshPro>();
             
            string str = nextNode.textLines[i].text;
            int indexOf = str.IndexOf("{0}");
            if (indexOf != -1)
            {
                correctTMPIndex = i;
                correctCharacterIndexBegin = indexOf;
                correctCharacterIndexEnd = indexOf + nextNode.statement.Length;
                str = string.Format(nextNode.textLines[i].text,
                    "<color=orange>" + nextNode.statement +
                    "</color>"); //  + ColorUtility.ToHtmlStringRGBA(nextDialogueNode.statementColor) +">" + nextDialogueNode.statement + 
            }

            tmp.text = str;
            StartCoroutine(FadeText(tmp, 0f, 1f, 0.2f));

            TextLine textLine = new TextLine(
                go,
                go.GetComponent<RectTransform>(),
                nextNode.textLines[i].textEffect,
                nextNode.textLines[i].ttl,
                go.GetComponent<TextMeshPro>(),
                correctTMPIndex,
                correctCharacterIndexBegin,
                correctCharacterIndexEnd
            );

            textLine.textMeshPro.ForceMeshUpdate();
            if (correctCharacterIndexBegin !=
                -1) // if there's a statement, build box colliders for the segment before, in and after the statement
            {
                CreateColliderAroundTextRange(textLine, 0, correctCharacterIndexBegin - 1, false);
                CreateColliderAroundTextRange(textLine, correctCharacterIndexBegin, correctCharacterIndexEnd, true);
                CreateColliderAroundTextRange(textLine, correctCharacterIndexEnd,
                    textLine.textMeshPro.textInfo.characterCount - 1, false);
            }
            else // if there's no statement, just build one box collider
            {
                CreateColliderAroundTextRange(textLine, 0, textLine.textMeshPro.textInfo.characterCount - 1, false);
            }

            textLines.Add(textLine);
        }
    }

    IEnumerator FadeText(TextMeshPro tmp, float from, float to, float duration)
    {
        Color color = tmp.color;
        color.a = from;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, elapsedTime / duration);
            tmp.color = color;
            yield return null;
        }

        color.a = to;
        tmp.color = color;
    }

    IEnumerator DestroyText(GameObject tmp)
    {
        yield return FadeText(tmp.GetComponent<TextMeshPro>(), 1f, 0f, 0.2f);
        Destroy(tmp);
    }

    // Gets the textLine to create for, the range and whether or not to make a child (AKA the orange part)
    public void CreateColliderAroundTextRange(TextLine textLine, int startIndex, int endIndex, bool createChildObject)
    {
        TextMeshPro tmp = textLine.textMeshPro;
        tmp.ForceMeshUpdate();
        TMP_TextInfo textInfo = tmp.textInfo;

        BoxCollider boxCollider = null;

        if (startIndex < 0 || endIndex > textInfo.characterCount || startIndex > endIndex)
        {
            return;
        }

        Vector3 min = Vector3.positiveInfinity;
        Vector3 max = Vector3.negativeInfinity;

        for (int i = startIndex; i <= endIndex; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            Vector3 bl = charInfo.bottomLeft;
            Vector3 tr = charInfo.topRight;

            min = Vector3.Min(min, bl);
            max = Vector3.Max(max, tr);
        }

        Vector3 center = (min + max) / 2;
        Vector3 size = max - min;

        if (createChildObject)
        {
            // Create the new GameObject
            GameObject orangeHitbox = new GameObject("OrangeHitBox");
            orangeHitbox.transform.SetParent(tmp.transform, false); // parent to text object
            orangeHitbox.transform.localPosition = Vector3.zero;
            orangeHitbox.transform.localRotation = Quaternion.identity;
            orangeHitbox.transform.localScale = Vector3.one;
            boxCollider = orangeHitbox.AddComponent<BoxCollider>();
            orangeHitbox.tag = "OrangeHitBox";
        }
        else
            boxCollider = textLine.textGO.AddComponent<BoxCollider>();

        boxCollider.center = center;
        boxCollider.size = size;
    }
}