using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Text = TMPro.TextMeshProUGUI;
public class GameLoop : MonoBehaviour
{
    class TextLine
    {
        public GameObject textGO;
        public RectTransform textRT;
        public List<TextEffect> textEffect;
        public float ttl;
        public TextMeshPro textMeshPro;    
       

        public TextLine(GameObject textGO, RectTransform textRT, List<TextEffect> textEffect, float ttl, TextMeshPro tmp)
        {
            this.textMeshPro = tmp;
            this.textGO = textGO;
            this.textRT = textRT;
            this.textEffect = textEffect;
            this.ttl = ttl;
        }

        internal void Apply()
        {
            for(int i = 0; i < textEffect.Count; i++)
            {
                textEffect[i].Apply(textRT);
            }
        }
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

    float timer;
    int textIndex;

    int correctTMPIndex, correctCharacterIndexBegin, correctCharacterIndexEnd;

    List<TextLine> textLines;
    CharacterStand characterStand;
    Evidence correctEvidence;

    bool pause;
    bool finished;
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPause(!pause);
        }
        if(pause == true || finished == true) { return; }
        if(stage.dialogueNodes.Count <= textIndex)
        {
            textIndex = 0;
        }

      
        timer += Time.deltaTime;
        stageTimer -= Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(stageTimer);
        timerText.text = timeSpan.ToString(@"mm\:ss\:ffff");

        if(stageTimer < 0f)
        {
            GameOver();
        }    

        effectController.Process();

        if(textLines.Count == 0)
        {
            SpawnText(textIndex);
            timer = 0;
            textIndex++;
        }

        int index = 0;
        while(index < textLines.Count)
        {
            if(textLines[index].ttl < timer)
            {
                Destroy(textLines[index].textGO);
                textLines.RemoveAt(index);
                index--;
            }
            index++;
        }

        if(textLines.Count > 0)
        {
            for(int i = 0; i < textLines.Count; i++)
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
        if(Input.GetMouseButtonDown(0))
        {
            ShootText();
            for (int i = 0; i < textLines.Count; i++)
            {
                if (correctTMPIndex != i)
                {
                    continue;
                }
                int index = TMP_TextUtilities.FindIntersectingCharacter(
                    textLines[i].textMeshPro,
                    Input.mousePosition,
                    Camera.main,
                    true
                    );
                if (index != -1)
                {
                    if(index >= correctCharacterIndexBegin && index < correctCharacterIndexEnd)
                    {
                        Hit();
                    }
                }
            }
        }
        
    }

    void SetPause(bool _pause)
    {
        pause = _pause;
        if(pause == true)
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
        Plane plane = new Plane(Camera.main.transform.forward, shootOrigin.position + Camera.main.transform.forward * 10f);

        if (plane.Raycast(ray, out float distance))
        {
          Vector3 targetPoint = ray.GetPoint(distance);
          Vector3 direction = (targetPoint - shootOrigin.position).normalized;

          Quaternion rotation = Quaternion.LookRotation(direction, Camera.main.transform.up) * Quaternion.Euler(0, 90, 0);
          GameObject bullet = Instantiate(textBulletPrefab, shootOrigin.position, rotation);

          Rigidbody rb = bullet.GetComponent<Rigidbody>();
          rb.velocity = direction * shootForce;

          Debug.DrawRay(shootOrigin.position, direction * 3, Color.red, 2f);


          Destroy(bullet, 3f);
        }
        

        // Quaternion spawnRotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(0, 90, 0); // rotate it to be parallel to the direction it's moving
        
        // GameObject bullet = Instantiate(textBulletPrefab, spawnPosition, spawnRotation);
        // Rigidbody rb = bullet.GetComponent<Rigidbody>();


        // if (rb != null)
        // {
        //     rb.velocity = direction * shootForce;
        // }

    }

    void Hit()
    {
        if (evidenceManager.Check(correctEvidence))
        {
            CorrectChoice();
        }
    }

    private void CorrectChoice()
    {
        finished = true;
        Debug.Log("You clicked on the statement" + Time.time);
    }

    void SpawnText(int dialogueNodeIndex)
    {
        correctTMPIndex = -1;
        correctCharacterIndexBegin = -1;
        correctCharacterIndexEnd = -1;

        DialogueNode nextDialogueNode = stage.dialogueNodes[dialogueNodeIndex];
        correctEvidence = nextDialogueNode.evidence;

        if(nextDialogueNode.character != null)
        {
            characterStand = characterStands.Find(
                stand => stand.character == nextDialogueNode.character
                );
            //find the transform of the new target for the camera
            cameraController.target = characterStand.spriteRenderer.transform;
            textPivot = characterStand.textPivot;
        }

        if(characterStand != null)
        {
            characterStand.state = stage.dialogueNodes[dialogueNodeIndex].expression;
            characterStand.SetSprite();
        }

        for(int i = 0; i < nextDialogueNode.textLines.Count; i++)
        {
            GameObject go = Instantiate(textPrefab);
            go.transform.position = textPivot.position;
            go.transform.position += nextDialogueNode.textLines[i].spawnOffset;
            go.transform.rotation = textPivot.rotation;
            go.transform.localScale = nextDialogueNode.textLines[i].scale/10;

            TextMeshPro tmp = go.GetComponent<TextMeshPro>();
            string str = nextDialogueNode.textLines[i].text;
            int indexOf = str.IndexOf("{0}");
            if(indexOf != -1)
            {
                correctTMPIndex = i;
                correctCharacterIndexBegin = indexOf;
                correctCharacterIndexEnd = indexOf + nextDialogueNode.statement.Length;
                str = string.Format(nextDialogueNode.textLines[i].text, "<color=orange>" + nextDialogueNode.statement +"</color>"); //  + ColorUtility.ToHtmlStringRGBA(nextDialogueNode.statementColor) +">" + nextDialogueNode.statement + 

            }
            tmp.text = str;

            TextLine textLine = new TextLine(
                go,
                go.GetComponent<RectTransform>(),
                nextDialogueNode.textLines[i].textEffect,
                nextDialogueNode.textLines[i].ttl,
                go.GetComponent<TextMeshPro>()
                );
            textLines.Add(textLine);
        }

       
        
       
        
        effectController.effect = nextDialogueNode.cameraEffect;
        effectController.Reset();
    }
}
