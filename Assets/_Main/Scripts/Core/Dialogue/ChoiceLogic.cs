using System;
using System.Collections;
using System.Collections.Generic;

public class Option
{
    public string text;
    public List<DialogueNode> dialogue;
    public bool isCorrect = false;
    
}
[Serializable]
public class ChoiceLogic
{
    public List<Option> options;
    public bool loopIfWrong = false;
    
    public ChoiceLogic()
    {
        options = new List<Option>();
    }

    public IEnumerator Play()
    {
        Option pickedOption;

        do
        {
            pickedOption = options[0];
            foreach (DialogueNode node in pickedOption.dialogue)
            {
                yield return node.Play();
            }
        } while (!pickedOption.isCorrect && loopIfWrong);
    }
}