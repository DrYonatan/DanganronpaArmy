using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public List<Evidence> evidenceList;
    
    public static EvidenceManager instance { get; private set; }
    
    public EvidenceMenu evidenceMenu;

    void Start()
    {
        instance = this;
        evidenceMenu.Initialize();
    }
    
    public IEnumerator AddEvidence(Evidence evidence)
    {
        evidenceList.Add(evidence);
        yield return evidenceMenu.OnEvidenceAdded(evidence);
    }

    public void RemoveEvidence(Evidence evidence)
    {
        evidenceList.Remove(evidence);
    }
}
