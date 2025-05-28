using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using System;

public class EvidenceManager : MonoBehaviour
{
    [SerializeField] Text textField;
    List<Evidence> evidences;
    int selectedIndex;

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedIndex++;
            if (selectedIndex >= evidences.Count)
            {
                selectedIndex = 0;
            }
            UpdateEvidence();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            selectedIndex--;
            if(selectedIndex < 0)
            {
                selectedIndex = evidences.Count-1;
            }
            UpdateEvidence();
        }
    }

    void UpdateEvidence()
    {
        string textToShow = "";
        for (int i = 0; i < this.evidences.Count; i++)
        {
            if (this.evidences[i] == null) { continue; }
            if(i == selectedIndex) { textToShow += ">>>"; }
            textToShow += this.evidences[i].Name + "\n";
        }
        textField.text = textToShow;
    }
    
    public void ShowEvidence(Evidence[] evidences)
    {
        this.evidences = evidences.ToList();
        this.evidences.RemoveAll(x => x == null);

        UpdateEvidence();
    }

    internal bool Check(Evidence correctEvidence)
    {
        return evidences[selectedIndex] == correctEvidence;
    }
}
