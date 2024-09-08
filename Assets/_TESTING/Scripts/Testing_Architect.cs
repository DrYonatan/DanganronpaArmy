using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING { 
public class Testing_Architect : MonoBehaviour
{

        DialogueSystem ds;
        TextArchitect architect;

        public TextArchitect.BuildMethod bm = TextArchitect.BuildMethod.instant;

        string[] lines = new string[3]
        {
            "קוראים לי (שם), אני שוטר צבאי ברמה על צבאית",
            "או בקיצור רע''צ",
            "שלום"
        };

    // Start is called before the first frame update
    void Start()
    {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.fade; 
    }

    // Update is called once per frame
    void Update()
    {
            if(bm != architect.buildMethod)
            {
                architect.buildMethod = bm;
                architect.Stop();
            }

            if (Input.GetKeyDown(KeyCode.S))
                architect.Stop();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                string line = "שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס שלומס  ";
               if (architect.buildProcess != null)   
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();
                }
                else
                    architect.Build(line);
                // architect.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if(Input.GetKeyDown(KeyCode.A))
                architect.Append(lines[Random.Range(0, lines.Length)]);
        }
}
}
