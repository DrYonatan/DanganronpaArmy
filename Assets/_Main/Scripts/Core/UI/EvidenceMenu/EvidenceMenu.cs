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
    public AddEvidenceAnimator animator;
    public GameObject mainContainer;
    public GameObject noEvidenceContainer;
    public CanvasGroup mainContainerCanvasGroup;
    public RectTransform infoContainerTransform;
    public RectTransform evidenceContainerTransform;
    public Image presentGuide;
    public Image closeupGuide;
    public Image closeupImage;
    public Image blackOverlay;
    public bool isCloseupOpen;
    public bool finishedTransition;
    private int infoContainerStartPosX;
    private int evidenceContainerStartPosY;

    public IEnumerator OnEvidenceAdded(Evidence evidence)
    {
        yield return animator.PlayAnimation(evidence);
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
        finishedTransition = true;
        presentGuide.gameObject.SetActive(false);
        closeupGuide.gameObject.SetActive(false);
        
        foreach (ListItem instantiated in evidenceListUI)
        {
            Destroy(instantiated.gameObject);
        }

        evidenceListUI.Clear();

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

    protected override void LoadContent()
    {
        base.LoadContent();
        StartAnimation();
    }

    void StartAnimation()
    {
        mainContainerCanvasGroup.alpha = 0;
        infoContainerTransform.DOAnchorPosX(infoContainerStartPosX + 300f, 0).SetUpdate(true);
        evidenceContainerTransform.DOAnchorPosY(evidenceContainerStartPosY - 300f, 0).SetUpdate(true);
        mainContainerCanvasGroup.DOFade(1f, 0.6f).SetUpdate(true);
        infoContainerTransform.DOAnchorPosX(infoContainerStartPosX, 0.6f).SetUpdate(true);
        evidenceContainerTransform.DOAnchorPosY(evidenceContainerStartPosY, 0.6f).SetUpdate(true);
    }

    void Update()
    {
        HandleCloseup();

        if (isCloseupOpen || !finishedTransition)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerInputManager.instance.pauseMenu.GoBackToGeneral();
        }

        if (evidenceListUI.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                currentEvidenceIndex = (currentEvidenceIndex + 1) % evidenceListUI.Count;
                UpdateUI();
                SoundManager.instance.PlaySoundEffect(moveSelectionSound);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                currentEvidenceIndex = (currentEvidenceIndex - 1 + evidenceListUI.Count) %
                                       evidenceListUI.Count;
                UpdateUI();
                SoundManager.instance.PlaySoundEffect(moveSelectionSound);
            }
        }
    }

    void HandleCloseup()
    {
        if (EvidenceManager.instance.evidenceList[currentEvidenceIndex].closeup != null && !isCloseupOpen &&
            finishedTransition &&
            PlayerInputManager.instance.DefaultInput())
        {
            isCloseupOpen = true;
            finishedTransition = false;
            blackOverlay.DOKill();
            closeupImage.DOKill();
            closeupImage.sprite = EvidenceManager.instance.evidenceList[currentEvidenceIndex].closeup;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(blackOverlay.DOFade(1f, 0.2f).SetUpdate(true));
            sequence.Append(closeupImage.DOFade(1f, 0f).SetUpdate(true));
            sequence.Append(blackOverlay.DOFade(0f, 0.2f).SetUpdate(true));
            sequence.OnComplete(() => finishedTransition = true);
            sequence.SetUpdate(true);
        }

        else if (isCloseupOpen && finishedTransition && Input.GetKeyDown(KeyCode.Escape))
        {
            isCloseupOpen = false;
            finishedTransition = false;
            blackOverlay.DOKill();
            closeupImage.DOKill();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(blackOverlay.DOFade(1f, 0.2f).SetUpdate(true));
            sequence.Append(closeupImage.DOFade(0f, 0f).SetUpdate(true));
            sequence.Append(blackOverlay.DOFade(0f, 0.2f).SetUpdate(true));
            sequence.OnComplete(() => finishedTransition = true);
            sequence.SetUpdate(true);
        }
    }

    void UpdateUI()
    {
        if (evidenceListUI.Count == 0)
        {
            mainContainer.SetActive(false);
            noEvidenceContainer.SetActive(true);
        }
        else
        {
            mainContainer.SetActive(true);
            noEvidenceContainer.SetActive(false);
            Evidence currentEvidence = EvidenceManager.instance.evidenceList[currentEvidenceIndex];
            if (currentEvidence != null)
            {
                evidenceIcon.sprite = currentEvidence.icon;
                evidenceIndexText.text =
                    $"{(currentEvidenceIndex + 1).ToString("00")}/{evidenceListUI.Count.ToString("00")}";
                evidenceDescription.text = currentEvidence.description;

                foreach (ListItem item in evidenceListUI)
                {
                    item.SetHovered(false);
                }
                
                closeupGuide.gameObject.SetActive(currentEvidence.closeup != null);

                if (evidenceListUI.Count > 0)
                    evidenceListUI[currentEvidenceIndex].SetHovered(true);

                evidenceListTransform.anchoredPosition = new Vector2(0, Mathf.Max((currentEvidenceIndex - 4) * 152, 0));
            }
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
        presentGuide.gameObject.SetActive(true);
        UpdateUI();

        bool isOpen = false;

        while (!Input.GetMouseButtonDown(1))
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
        presentGuide.gameObject.SetActive(false);
        Evidence currentEvidence = EvidenceManager.instance.evidenceList[currentEvidenceIndex];
        yield return onFinish(currentEvidence);
    }
}