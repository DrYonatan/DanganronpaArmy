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

    List<TextLine> textLines;
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
        textLines = new List<TextLine>();
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
        
            if (textLines.Count == 0)
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
            while (index < textLines.Count)
            {
                if (textLines[index].ttl < timer)
                {
                    StartCoroutine(DestroyText(textLines[index].textGO));
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
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
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
        if(isActive)
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
        foreach (TextLine text in textLines)
        {
            gameObject.GetComponent<TextShatterEffect>().Shatter(text);
        }

        OnHitStatement();
        CursorManager.instance.Hide();
    }

    public void WrongHit()
    {
        foreach(TextLine text in textLines)
        {
            StartCoroutine(DestroyText(text.textGO));
        }
        textLines.Clear();
        DeactivateDebate();
        StartCoroutine(PlayWrongHitNodes());
    }

    IEnumerator PlayWrongHitNodes()
    {
        List<DiscussionNode> wrongNodes = UtilityNodesRuntimeBank.instance.nodesCollection.characterDefaultWrongNodes.Find(
            item => item.character == debateSegment.dialogueNodes[textIndex-1 >= 0 ? textIndex - 1 : debateSegment.dialogueNodes.Count-1].character).nodes;
        
        
        yield return SwitchToTextBoxMode();
        yield return TrialDialogueManager.instance.RunNodes(wrongNodes);
        yield return new WaitForEndOfFrame();
        debateUIAnimator.FadeFromAngleToAngle();
        TrialManager.instance.DecreaseHealth(1f);
        yield return TrialDialogueManager.instance.RunNodes(UtilityNodesRuntimeBank.instance.nodesCollection.debateWrongEvidence);
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
        yield return cameraController.SpinToTarget(characterStand.transform, characterStand.heightPivot, nextNode.positionOffset, nextNode.rotationOffset, nextNode.fovOffset);
       
        foreach (CameraEffect cameraEffect in nextNode.cameraEffects)
        { 
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
            characterStand = TrialManager.instance.characterStands.Find(stand => stand.character == nextNode.character);
        }

        if (characterStand != null)
        {
            characterStand.SetSprite(nextNode.character.emotions[nextNode.expressionIndex]);
        }

        DebateTextData textData = nextNode.textData as DebateTextData;
        for (int i = 0; i < textData.textLines.Count; i++)
        {
            correctTMPIndex = -1;
            correctCharacterIndexBegin = -1;
            correctCharacterIndexEnd = -1;

            GameObject go = Instantiate(textPrefab, statementsCamera.transform.parent);
            go.transform.position = textStartPosition.position;
            go.transform.position += textData.textLines[i].spawnOffset;
            go.transform.localScale = textData.textLines[i].scale;

            TextMeshPro tmp = go.GetComponent<TextMeshPro>();
             
            string str = textData.textLines[i].text;
            int indexOf = str.IndexOf("{0}");
            if (indexOf != -1)
            {
                correctTMPIndex = i;
                correctCharacterIndexBegin = indexOf;
                correctCharacterIndexEnd = indexOf + nextNode.statement.Length;
                str = string.Format(textData.textLines[i].text,
                    "<color=orange>" + nextNode.statement +
                    "</color>"); //  + ColorUtility.ToHtmlStringRGBA(nextDialogueNode.statementColor) +">" + nextDialogueNode.statement + 
            }

            tmp.text = str;
            StartCoroutine(FadeText(tmp, 0f, 1f, 0.2f));

            TextLine textLine = new TextLine(
                go,
                go.GetComponent<RectTransform>(),
                textData.textLines[i].textEffect,
                textData.textLines[i].ttl,
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

    public int GetSelectedEvidenceIndex()
    {
        return evidenceManager.selectedIndex;
    }
}