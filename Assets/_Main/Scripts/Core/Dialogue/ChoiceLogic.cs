using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DIALOGUE;
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

    public IEnumerator Play(Func<IEnumerator> playOriginalNode)
    {
        Option<T> pickedOption = new Option<T>();

        do
        {
            yield return playOriginalNode();
            
            yield return DialogueSystem.instance.HandleSelection(options, (selectedOption) =>
            {
                pickedOption = selectedOption;
            });
            
            foreach (T node in pickedOption.dialogue)
            {
                yield return node.Play();
            }  
           
        } while (!pickedOption.isCorrect && loopIfWrong);
    }
}