using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EvidenceMenu : MenuScreen
{
    public int currentEvidenceIndex;
    public Image evidenceIcon;
    public TextMeshProUGUI evidenceIndexText;
    public VerticalLayoutGroup evidenceContainer;
    [FormerlySerializedAs("evidenceItem")] public ListItem listItem;
    public List<ListItem> evidenceListUI = new List<ListItem>();
    public TextMeshProUGUI evidenceDescription;
    public RectTransform evidenceListTransform;
    public AudioClip moveSelectionSound;
    public QuestionBubble questionBubble;
    public TextMeshProUGUI questionBubbleText;

    public void OnEvidenceAdded(Evidence evidence)
    {
        AddEvidenceToList(evidence);
    }

    void AddEvidenceToList(Evidence evidence)
    {
        ListItem instantiated = Instantiate(listItem);
        instantiated.SetText(evidence.Name);
        instantiated.transform.SetParent(evidenceContainer.transform, false);
        evidenceListUI.Add(instantiated);
    }

    public void Initialize()
    {
        foreach (Evidence evidence in EvidenceManager.instance.evidenceList)
        {
            AddEvidenceToList(evidence);
        }

        UpdateUI();
    }

    public override void Open()
    {
        base.Open();
        currentEvidenceIndex = 0;
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentEvidenceIndex = (currentEvidenceIndex + 1) % EvidenceManager.instance.evidenceList.Count;
            UpdateUI();
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            currentEvidenceIndex = (currentEvidenceIndex - 1 + EvidenceManager.instance.evidenceList.Count) %
                                   EvidenceManager.instance.evidenceList.Count;
            UpdateUI();
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
        }
        
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerInputManager.instance.pauseMenu.GoBackToGeneral();
        }
    }

    void UpdateUI()
    {
        Evidence currentEvidence = EvidenceManager.instance.evidenceList.Count > 0 ? EvidenceManager.instance.evidenceList[currentEvidenceIndex] : null;
        if (currentEvidence != null)
        {
            evidenceIcon.sprite = currentEvidence.icon;
            evidenceIndexText.text =
                $"{(currentEvidenceIndex + 1).ToString("00")}/{EvidenceManager.instance.evidenceList.Count.ToString("00")}";
            evidenceDescription.text = currentEvidence.description;

            foreach (ListItem item in evidenceListUI)
            {
                item.SetHovered(false);
            }

            if (evidenceListUI.Count > 0)
                evidenceListUI[currentEvidenceIndex].SetHovered(true);

            evidenceListTransform.anchoredPosition = new Vector2(0, Mathf.Max((currentEvidenceIndex - 5) * 91, 0));
        }
    }

    public IEnumerator SelectEvidence(string question, Func<Evidence, IEnumerator> onFinish)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack).SetUpdate(true);
        content.SetActive(true);
        logo.alpha = 0f;
        gameObject.SetActive(true);
        currentEvidenceIndex = 0;
        UpdateUI();
        
        bool isOpen = false;

        while (PlayerInputManager.instance.DefaultInput())
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                isOpen = true;
                questionBubble.Open();
                questionBubbleText.text = question;
            }
            else
            {
                if (isOpen)
                {
                    questionBubble.Close();
                }
                
                isOpen = false;
            }
            yield return null;
        }
        
        questionBubble.gameObject.SetActive(false);

        Close();
        Evidence currentEvidence = EvidenceManager.instance.evidenceList[currentEvidenceIndex];
        yield return onFinish(currentEvidence);
    }
}