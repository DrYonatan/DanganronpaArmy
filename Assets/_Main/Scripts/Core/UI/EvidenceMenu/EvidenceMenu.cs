using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceMenu : MonoBehaviour
{
    public int currentEvidenceIndex;
    public Image evidenceIcon;
    public TextMeshProUGUI evidenceIndexText;
    public VerticalLayoutGroup evidenceContainer;
    public EvidenceItem evidenceItem;
    public List<EvidenceItem> evidenceListUI = new List<EvidenceItem>();
    public TextMeshProUGUI evidenceDescription;

    public void OnEvidenceAdded(Evidence evidence)
    {
        AddEvidenceToList(evidence);
    }

    void AddEvidenceToList(Evidence evidence)
    {
        EvidenceItem instantiated = Instantiate(evidenceItem);
        instantiated.SetText(evidence.Name);
        instantiated.transform.SetParent(evidenceContainer.transform, false);
        evidenceListUI.Add(instantiated);
    }

    void Start()
    {
        foreach (Evidence evidence in EvidenceManager.instance.evidenceList)
        {
            AddEvidenceToList(evidence);
        }

        UpdateUI();
    }

    void Awake()
    {
        currentEvidenceIndex = 0;
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentEvidenceIndex = (currentEvidenceIndex + 1) % EvidenceManager.instance.evidenceList.Count;
            UpdateUI();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            currentEvidenceIndex = (currentEvidenceIndex - 1 + EvidenceManager.instance.evidenceList.Count) %
                                   EvidenceManager.instance.evidenceList.Count;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        Evidence currentEvidence = EvidenceManager.instance.evidenceList[currentEvidenceIndex];
        if (currentEvidence != null)
        {
            evidenceIcon.sprite = currentEvidence.icon;
            evidenceIndexText.text =
                $"{(currentEvidenceIndex + 1).ToString("00")}/{EvidenceManager.instance.evidenceList.Count.ToString("00")}";
            evidenceDescription.text = currentEvidence.description;

            foreach (EvidenceItem item in evidenceListUI)
            {
                item.isHovered = false;
            }

            if (evidenceListUI.Count > 0)
                evidenceListUI[currentEvidenceIndex].isHovered = true;
        }
    }
}