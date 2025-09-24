using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Option<T> where T : DialogueNode
{
    public string text;
    [SerializeReference] public List<T> dialogue = new List<T>();
    public bool isCorrect = false;
    
}
[Serializable]
public class ChoiceLogic<T> where T : DialogueNode
{
    public List<Option<T>> options;
    public bool loopIfWrong = false;
    
    public ChoiceLogic()
    {
        options = new List<Option<T>>();
    }

    public IEnumerator Play()
    {
        Option<T> pickedOption;

        do
        {
            pickedOption = options[0];
            foreach (T node in pickedOption.dialogue)
            {
                yield return node.Play();
            }
        } while (!pickedOption.isCorrect && loopIfWrong);
    }
}