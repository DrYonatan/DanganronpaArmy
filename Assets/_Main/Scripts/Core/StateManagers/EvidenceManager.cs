using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EvidenceManager : MonoBehaviour
{
    public List<Evidence> evidenceList;
    
    public static EvidenceManager instance { get; private set; }
    
    public EvidenceMenu evidenceMenu;

    void Start()
    {
        instance = this;
    }
    
    public void AddEvidence(Evidence evidence)
    {
        evidenceList.Add(evidence);
        evidenceMenu.OnEvidenceAdded(evidence);
    }

    public void RemoveEvidence(Evidence evidence)
    {
        evidenceList.Remove(evidence);
    }
}
