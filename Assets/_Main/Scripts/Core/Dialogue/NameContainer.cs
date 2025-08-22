using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace DIALOGUE
{ 
    [System.Serializable]
public class NameContainer  
{
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI nameText;

    public void Show(string nameToShow = "")
    {
        if (nameToShow != string.Empty)
            nameText.text = nameToShow;
    }

    public void Clear()
    {
        nameText.text = "";
    }
}
}
