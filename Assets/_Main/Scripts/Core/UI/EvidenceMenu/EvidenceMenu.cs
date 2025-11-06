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

    public void OnEvidenceAdded(Evidence evidence)
    {
        AddEvidenceToList(evidence);
    }

    void AddEvidenceToList(Evidence evidence)
    {
        EvidenceItem instantiated = Instantiate(evidenceItem);
        instantiated.SetText(evidence.Name);
        instantiated.transform.SetParent(evidenceContainer.transform, false);
    }

    void Start()
    {
       
        foreach (Evidence evidence in EvidenceManager.instance.evidenceList)
        {
            AddEvidenceToList(evidence);
        }
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
            currentEvidenceIndex++;
            UpdateUI();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            currentEvidenceIndex--;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        evidenceIcon.sprite = EvidenceManager.instance.evidenceList[currentEvidenceIndex].icon;
        evidenceIndexText.text =
            $"{(currentEvidenceIndex + 1).ToString("00")}/{EvidenceManager.instance.evidenceList.Count.ToString("00")}";
    }
}