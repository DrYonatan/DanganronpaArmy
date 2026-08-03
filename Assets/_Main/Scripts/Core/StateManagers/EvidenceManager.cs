using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public List<Evidence> evidenceList;
    
    public static EvidenceManager instance { get; private set; }
    
    public EvidenceMenu evidenceMenu;

    void Start()
    {
        instance = this;
    }

    public void Initialize(List<Evidence> evidence)
    {
        evidenceList = evidence;
        evidenceMenu.Initialize();
    }
    
    public IEnumerator AddEvidence(Evidence evidence)
    {
        if (!evidenceList.Contains(evidence))
        {
            evidenceList.Add(evidence);
            yield return evidenceMenu.OnEvidenceAdded(evidence); 
        }
        else
           DialogueSystem.instance.TurnOnSingleTimeAuto();

    }

    public void RemoveEvidence(Evidence evidence)
    {
        evidenceList.Remove(evidence);
    }
}
