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


    // Start is called before the first frame update
    void Start()
    {
        textLines = new List<TextLine>();
        evidenceManager.ShowEvidence(stage.evidences);
        MusicManager.instance.PlaySong(stage.audioClip.name);
        stageTimer = defaultStageTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPause(!pause);
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

        effectController.Process();

        if (textLines.Count == 0)
        {
            SpawnText(textIndex);
            timer = 0;
            textIndex++;
        }

        int index = 0;
        while (index < textLines.Count)
        {
            if (textLines[index].ttl < timer)
            {
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

    private void GameOver()
    {
        finished = true;
    }

    void HandleMouseControl()
    {
        ReticleManager.instance.ReticleAsCursor();
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
        // Use the shootOrigin or fallback to camera's position
        Vector3 spawnPosition = shootOrigin ? shootOrigin.position : Camera.main.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a plane in front of the firePoint (facing the same way as the camera)
        Plane plane = new Plane(Camera.main.transform.forward,
            shootOrigin.position + Camera.main.transform.forward * 4f);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - shootOrigin.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction, Camera.main.transform.up) *
                                  Quaternion.Euler(0, 90f, 0);
            GameObject bullet = Instantiate(textBulletPrefab, shootOrigin.position, rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

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
        ReticleManager.instance.Hide();
    }

    public void OnHitStatement()
    {
        StartCoroutine(DebateHitEffect());
    }

    IEnumerator DebateHitEffect()
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 startPos = cameraTransform.position;
        Vector3 forwardLocation = -cameraTransform.forward;
        Vector3 targetPosition = startPos + forwardLocation;
        Quaternion targetRotation = cameraTransform.rotation * Quaternion.Euler(0f, 15f, 0f);
        Quaternion oppositeRotation = cameraTransform.rotation * Quaternion.Euler(0f, -5f, 0f);
        StartCoroutine(PlayNoThatsWrong(1.5f));
        yield return cameraController.MoveCameraOnXAndZ(targetPosition, targetRotation, 0.2f);
        yield return cameraController.MoveCameraOnXAndZ(startPos - forwardLocation, targetRotation, 0.2f);
        StartCoroutine(cameraController.MoveCameraOnXAndZ(targetPosition, oppositeRotation, 4f));
        yield return new WaitForSeconds(3f);
        shatterTransform.SetActive(true);
    }

    IEnumerator PlayNoThatsWrong(float delay)
    {
        yield return new WaitForSeconds(delay);

        noThatsWrong.SetActive(true);
        SoundManager.instance.PlaySoundEffect("nothatswrong");

        yield return new WaitForSeconds(3f);

        noThatsWrong.SetActive(false);
    }

    void SpawnText(int dialogueNodeIndex)
    {
        int correctTMPIndex = -1;
        int correctCharacterIndexBegin = -1;
        int correctCharacterIndexEnd = -1;

        DebateNode nextNode = stage.dialogueNodes[dialogueNodeIndex];
        correctEvidence = nextNode.evidence;

        if (nextNode.character != null)
        {
            characterStand = characterStands.Find(stand => stand.character == nextNode.character
            );
            //find the transform of the new target for the camera
            cameraController.target = characterStand.spriteRenderer.transform;
            textPivot = characterStand.textPivot;
        }

        if (characterStand != null)
        {
            characterStand.state = stage.dialogueNodes[dialogueNodeIndex].expression;
            characterStand.SetSprite();
        }

        for (int i = 0; i < nextNode.textLines.Count; i++)
        {
            correctTMPIndex = -1;
            correctCharacterIndexBegin = -1;
            correctCharacterIndexEnd = -1;

            GameObject go = Instantiate(textPrefab);
            go.transform.position = textPivot.position;
            go.transform.position += nextNode.textLines[i].spawnOffset;
            go.transform.rotation = textPivot.rotation;
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

        effectController.effect = nextNode.cameraEffect;
        effectController.Reset();
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
            orangeHitbox.transform.localPosition = center;
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