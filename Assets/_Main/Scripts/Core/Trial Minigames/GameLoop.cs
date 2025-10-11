using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using _Main.Scripts.Court;
using DIALOGUE;
using Text = TMPro.TextMeshProUGUI;

public class GameLoop : MonoBehaviour
{
    public class FloatingText
    {
        public List<GameObject> linesGameObjects;
        public Transform textTransform;
        public List<TextEffect> textEffects;
        public float ttl;
        public List<TextMeshPro> linesTextMeshPros;
        public int correctCharacterIndexBegin, correctCharacterIndexEnd;

        public FloatingText(List<GameObject> linesGameObjects, Transform textTransform, List<TextEffect> textEffects,
            float ttl,
            List<TextMeshPro> linesTextMeshPros, int correctCharacterIndexBegin, int correctCharacterIndexEnd)
        {
            this.linesTextMeshPros = linesTextMeshPros;
            this.linesGameObjects = linesGameObjects;
            this.textTransform = textTransform;
            this.textEffects = textEffects;
            this.ttl = ttl;
            this.correctCharacterIndexBegin = correctCharacterIndexBegin;
            this.correctCharacterIndexEnd = correctCharacterIndexEnd;
        }

        internal void Apply()
        {
            foreach (TextEffect effect in textEffects)
            {
                effect.Apply(textTransform);
            }
        }
    }


    public static GameLoop instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] CameraController cameraController;
    public DebateSegment debateSegment;
    [SerializeField] Transform textPivot;
    [SerializeField] GameObject textPrefab;
    [SerializeField] CameraEffectController effectController;
    [SerializeField] EvidenceManager evidenceManager;
    [SerializeField] MusicManager musicManager;
    [SerializeField] Text timerText;
    public DebateUIAnimator debateUIAnimator;
    public bool isShooting;
    public NoThatsWrongAnimator noThatsWrong;
    private bool reachedEnd = false;

    float timer;
    int textIndex;

    List<FloatingText> debateTexts;
    CharacterStand characterStand;
    Evidence correctEvidence;

    bool pause;
    bool isActive = false;
    float stageTimer;
    float defaultStageTime = 600f;

    public GameObject textBulletPrefab;
    public float shootForce = 10f;
    public Transform shootOrigin;
    public Transform textStartPosition;
    public Camera statementsCamera;
    public TrialHoverable currentAimedText;
    public Camera renderTextureCamera;
    public ScreenShatterManager screenShatter;

    public void PlayDebate(DebateSegment debate)
    {
        this.debateSegment = debate;
        debateTexts = new List<FloatingText>();
        evidenceManager.ShowEvidence(debateSegment.settings.evidences);
        MusicManager.instance.PlaySong(debateSegment.settings.audioClip);
        stageTimer = defaultStageTime;
        StartCoroutine(StartDebate());
    }

    IEnumerator StartDebate()
    {
        DialogueSystem.instance.SetTextBox(debateUIAnimator.dialogueContainer);
        yield return cameraController.DiscussionOutroMovement(2.5f);
        debateUIAnimator.gameObject.SetActive(true);
        debateUIAnimator.DebateUIDisappear();
        ((CourtTextBoxAnimator)(DialogueSystem.instance.dialogueBoxAnimator)).ChangeFace(null);
        yield return 0;
        ImageScript.instance.UnFadeToBlack(1f);
        yield return StartCoroutine(cameraController.DebateStartCameraMovement(3f));
        isActive = true;
        TimeManipulationManager.instance.isInputActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetPause(!pause);
            }

            if (pause || !isActive)
            {
                return;
            }

            if (debateSegment.dialogueNodes.Count <= textIndex)
            {
                textIndex = 0;
                reachedEnd = true;
            }


            timer += Time.deltaTime;
            stageTimer -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(stageTimer);
            timerText.text = timeSpan.ToString(@"mm\:ss\:ffff");

            if (stageTimer < 0f)
            {
                GameOver();
            }

            if (debateTexts.Count == 0)
            {
                if (reachedEnd)
                {
                    debateUIAnimator.UnHighlightAllNodes();
                    DeactivateDebate();
                    StartCoroutine(StartFinishNodes(debateSegment.finishNodes));
                }
                else
                {
                    StartCoroutine(StartNewNode(textIndex));
                    timer = 0;
                    textIndex++;
                }
            }

            int index = 0;
            while (index < debateTexts.Count)
            {
                if (debateTexts[index].ttl < timer)
                {
                    StartCoroutine(DestroyText(debateTexts[index]));
                    debateTexts.RemoveAt(index);
                    index--;
                }

                index++;
            }

            if (debateTexts.Count > 0)
            {
                foreach (FloatingText text in debateTexts)
                {
                    text.Apply();
                }
            }

            if (!isShooting)
            {
                HandleBulletMenuOpening();
                HandleMouseScroll();
                HandleMouseControl();
            }
        }
    }

    void DeactivateDebate()
    {
        Time.timeScale = 1f;
        isActive = false;
        TimeManipulationManager.instance.DeActivateInput();
    }


    IEnumerator StartFinishNodes(List<DiscussionNode> finishNodes)
    {
        yield return SwitchToTextBoxMode();
        yield return TrialDialogueManager.instance.RunNodes(finishNodes);
        yield return SwitchToDebateMode();
    }

    IEnumerator SwitchToTextBoxMode()
    {
        debateUIAnimator.HideCylinderAndCircles();
        CursorManager.instance.Hide();
        debateUIAnimator.ChangeFace(null);
        DialogueSystem.instance.ClearTextBox();
        yield return new WaitForSeconds(1f);
        yield return new WaitForEndOfFrame();
        debateUIAnimator.FadeFromAngleToAngle();
        debateUIAnimator.ShowTextBox();
    }

    IEnumerator SwitchToDebateMode()
    {
        debateUIAnimator.ChangeFace(null);
        DialogueSystem.instance.ClearTextBox();
        debateUIAnimator.HideTextBox();
        yield return new WaitForSeconds(1f);
        reachedEnd = false;
        isActive = true;
        TimeManipulationManager.instance.isInputActive = true;
        debateUIAnimator.ShowCylinderAndCircles();
        CursorManager.instance.Show();
    }


    private void GameOver()
    {
        DeactivateDebate();
    }

    void HandleMouseControl()
    {
        TrialCursorManager.instance.ReticleAsCursor();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        if (Input.GetMouseButtonDown(0))
        {
            ShootText();
        }
    }

    void HandleMouseScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            evidenceManager.SelectNextEvidence();
            evidenceManager.SelectNextEvidence();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            evidenceManager.SelectPreviousEvidence();
        }
    }

    void HandleBulletMenuOpening()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            debateUIAnimator.OpenBulletSelectionMenu();
        }
        else
        {
            debateUIAnimator.CloseBulletSelectionMenu();
        }
    }

    void SetPause(bool _pause)
    {
        pause = _pause;
        if (pause)
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
        if (!isShooting)
        {
            isShooting = true;
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
                evidenceManager.ShootBullet();
                GameObject bullet = Instantiate(textBulletPrefab, shootOrigin.position, rotation);
                bullet.GetComponent<TextMeshPro>().text = evidenceManager.GetSelectedEvidence();
                StartCoroutine(MoveBullet(bullet, direction, 1f));
                debateUIAnimator.MoveCylinder();
                debateUIAnimator.GrowAndShrinkCircles();
            }
        }
    }

    IEnumerator MoveBullet(GameObject bullet, Vector3 direction, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            bullet.transform.position += direction * (shootForce * Time.unscaledDeltaTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(bullet);
        isShooting = false;
        if (isActive)
            debateUIAnimator.LoadBullet();
    }

    public void LoadBullets()
    {
        evidenceManager.LoadBullets();
    }

    public void Hit(Vector3 point)
    {
        CorrectChoice();
        gameObject.GetComponent<TextShatterEffect>().Explosion(point);
    }

    public bool CheckEvidence()
    {
        return evidenceManager.Check(correctEvidence);
    }

    private void CorrectChoice()
    {
        DeactivateDebate();
        foreach (FloatingText text in debateTexts)
        {
            foreach (TextMeshPro lineTMP in text.linesTextMeshPros)
            {
                gameObject.GetComponent<TextShatterEffect>().Shatter(lineTMP);
            }
        }

        OnHitStatement();
        CursorManager.instance.Hide();
    }

    public void WrongHit()
    {
        foreach (FloatingText text in debateTexts)
        {
            StartCoroutine(DestroyText(text));
        }

        debateTexts.Clear();
        DeactivateDebate();
        StartCoroutine(PlayWrongHitNodes());
    }

    IEnumerator PlayWrongHitNodes()
    {
        List<DiscussionNode> wrongNodes = UtilityNodesRuntimeBank.instance.nodesCollection.characterDefaultWrongNodes
            .Find(item => item.character == debateSegment
                .dialogueNodes[textIndex - 1 >= 0 ? textIndex - 1 : debateSegment.dialogueNodes.Count - 1].character)
            .nodes;


        yield return SwitchToTextBoxMode();
        yield return TrialDialogueManager.instance.RunNodes(wrongNodes);
        yield return new WaitForEndOfFrame();
        debateUIAnimator.FadeFromAngleToAngle();
        TrialManager.instance.DecreaseHealth(1f);
        yield return TrialDialogueManager.instance.RunNodes(UtilityNodesRuntimeBank.instance.nodesCollection
            .debateWrongEvidence);
        CharacterStand characterStand =
            TrialManager.instance.characterStands.Find((stand) => stand.character.name == "Alon");
        characterStand.SetSprite(characterStand.character.emotions[1]);
        textIndex = 0;
        yield return SwitchToDebateMode();
    }


    void OnHitStatement()
    {
        StartCoroutine(DebateHitEffect());
    }

    IEnumerator DebateHitEffect()
    {
        effectController.Reset();
        Vector3 firstTargetPosition = new Vector3(1f, 4.2f, -8f);
        Vector3 secondTargetPosition = firstTargetPosition - new Vector3(0f, 0f, 8f);
        StartCoroutine(PlayNoThatsWrong(1.5f));
        StartCoroutine(cameraController.ChangeFov(cameraController.camera.fieldOfView, 8, 0.7f));
        yield return cameraController.MoveCamera(firstTargetPosition, Quaternion.Euler(0f, -5f, 0f), 0.4f);
        StartCoroutine(cameraController.MoveCamera(secondTargetPosition, Quaternion.Euler(0f, 0f, 30f), 4f));
        yield return new WaitForSeconds(3.6f);
        cameraController.camera.targetTexture = null;
        screenShatter = Instantiate(screenShatter);
        yield return StartCoroutine(screenShatter.ScreenShatter());
        ImageScript.instance.FadeToBlack(0.01f);
        yield return new WaitForSeconds(0.01f);

        ImageScript.instance.UnFadeToBlack(0.5f);
        yield return cameraController.DiscussionIntroMovement(1f);
        musicManager.StopSong();
        debateSegment.Finish();
    }

    IEnumerator PlayNoThatsWrong(float delay)
    {
        yield return new WaitForSeconds(delay);

        noThatsWrong.gameObject.SetActive(true);
        yield return noThatsWrong.Show();
        noThatsWrong.gameObject.SetActive(false);
    }

    IEnumerator StartNewNode(int dialogueNodeIndex)
    {
        Character prevCharacter = ScriptableObject.CreateInstance<Character>();
        if (dialogueNodeIndex > 0)
        {
            prevCharacter = debateSegment.dialogueNodes[dialogueNodeIndex - 1].character;
        }

        DebateNode nextNode = debateSegment.dialogueNodes[dialogueNodeIndex];
        SpawnText(nextNode);
        effectController.Reset();

        if (dialogueNodeIndex == 0 || prevCharacter != nextNode.character)
        {
            debateUIAnimator.ChangeFace(nextNode.character.faceSprite);
        }


        debateUIAnimator.UpdateName(nextNode.character.displayName);
        debateUIAnimator.HighlightNode(textIndex);
        yield return cameraController.SpinToTarget(characterStand.transform, characterStand.heightPivot,
            nextNode.positionOffset, nextNode.rotationOffset, nextNode.fovOffset);

        foreach (CameraEffect cameraEffect in nextNode.cameraEffects)
        {
            effectController.StartEffect(cameraEffect);
        }
    }

    string[] AddOrangeToText(string[] strings, int orangeStartIndex, int orangeEndIndex)
    {
        string[] results =  new string[strings.Length];
        int overallIndex = 0;
        for (int j = 0; j < strings.Length; j++)
        {
            results[j] = strings[j];
            int statementStartIndex = strings[j].IndexOf("{");
            if (statementStartIndex != -1)
            {
                results[j] = strings[j].Replace("{", "<color=orange>");
            }
            else if (overallIndex <= orangeEndIndex && overallIndex >= orangeStartIndex)
            {
                results[j] = "<color=orange>" + strings[j];
            }


            int statementEndIndex = strings[j].IndexOf("}");
            if (statementEndIndex != -1)
            {
                results[j] = results[j].Replace("}", "</color>");
            }

            overallIndex += strings[j].Length;
        }

        return results;
    }

    GameObject GenerateTextLine(Transform parent, string text, int index)
    {
        GameObject textLine = Instantiate(textPrefab, parent);
        textLine.transform.localPosition += index * 0.2f * Vector3.down;
        
        TextMeshPro tmp = textLine.GetComponent<TextMeshPro>();
        tmp.text = text;
        StartCoroutine(FadeText(tmp, 0f, 1f, 0.2f));

        return textLine;
    }

    void GenerateTextLines(TextLine nodeText)
    {
        int correctCharacterIndexBegin = -1;
        int correctCharacterIndexEnd = -1;

        GameObject go = new GameObject();
        GameObject instantiated = Instantiate(go, statementsCamera.transform.parent);
        instantiated.transform.position = textStartPosition.position + nodeText.spawnOffset;;
        instantiated.transform.localScale = nodeText.scale;
        
        string str = nodeText.text;
        int orangeStartIndex = str.IndexOf("{");
        if (orangeStartIndex != -1)
        {
            correctCharacterIndexBegin = orangeStartIndex;
            correctCharacterIndexEnd = str.IndexOf("}");
        }

        string[] results = AddOrangeToText( str.Split("\n"), correctCharacterIndexBegin, correctCharacterIndexEnd);

        List<GameObject> textLineObjects = new List<GameObject>();

        for (int i = 0; i < results.Length; i++)
        {
            GameObject textLine = GenerateTextLine(instantiated.transform, results[i], i);
            textLineObjects.Add(textLine);
            
            GenerateTextLineColliders(textLine.GetComponent<TextMeshPro>());
        }
        

        FloatingText floatingText = new FloatingText(
            textLineObjects,
            instantiated.transform,
            nodeText.textEffect,
            nodeText.ttl,
            textLineObjects.ConvertAll(x => x.GetComponent<TextMeshPro>()),
            correctCharacterIndexBegin,
            correctCharacterIndexEnd
        );

      

        debateTexts.Add(floatingText);
    }

    void GenerateTextLineColliders(TextMeshPro textLine)
    {
        int orangeStartIndex = textLine.text.IndexOf("<color=orange>");
        int orangeEndIndex = textLine.text.IndexOf("</color>");
        
        textLine.ForceMeshUpdate();
        
        if (orangeEndIndex == -1)
        {
            orangeEndIndex =  textLine.textInfo.characterCount + "<color=orange>".Length;
        }
        
        if (orangeStartIndex != -1) // if there's a statement, build box colliders for the segment before, in and after the statement
        {
            CreateColliderAroundTextRange(textLine.gameObject, 0, orangeStartIndex-1, false);
            CreateColliderAroundTextRange(textLine.gameObject, orangeStartIndex, orangeEndIndex-"<color=orange>".Length, true);
            CreateColliderAroundTextRange(textLine.gameObject, orangeEndIndex-"<color=orange>".Length,
                textLine.textInfo.characterCount - 1, false);
        }
        else // if there's no statement, just build one box collider
        {
            CreateColliderAroundTextRange(textLine.gameObject, 0, textLine.textInfo.characterCount - 1, false);
        }
    }

    void SpawnText(DebateNode nextNode)
    {
        correctEvidence = nextNode.evidence;

        if (nextNode.character != null)
        {
            characterStand = TrialManager.instance.characterStands.Find(stand => stand.character == nextNode.character);
        }

        if (characterStand != null)
        {
            characterStand.SetSprite(nextNode.character.emotions[nextNode.expressionIndex]);
        }

        DebateTextData textData = nextNode.textData as DebateTextData;
        for (int i = 0; i < textData.textLines.Count; i++)
        {
            GenerateTextLines(textData.textLines[i]);
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

    IEnumerator DestroyText(FloatingText text)
    {
        float duration = 0.2f;

        foreach (TextMeshPro line in text.linesTextMeshPros)
        {
            StartCoroutine(FadeText(line, 1f, 0f, duration));
        }

        yield return new WaitForSeconds(duration);

        Destroy(text.textTransform.gameObject);
    }

    // Gets the textLine to create for, the range and whether or not to make a child (AKA the orange part)
    void CreateColliderAroundTextRange(GameObject textGameObject,  int startIndex, int endIndex, bool createChildObject)
    {
        TextMeshPro tmp =  textGameObject.GetComponent<TextMeshPro>();
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
            boxCollider = textGameObject.AddComponent<BoxCollider>();

        boxCollider.center = center;
        boxCollider.size = size;
    }

    public int GetSelectedEvidenceIndex()
    {
        return evidenceManager.selectedIndex;
    }
}