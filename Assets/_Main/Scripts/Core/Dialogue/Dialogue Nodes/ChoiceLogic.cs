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

    public IEnumerator Play(Func<IEnumerator> playOriginalNode, Action onCorrect, Action onWrong)
    {
        Option<T> pickedOption = new Option<T>();

        do
        {
            DialogueSystem.instance.SetAuto(true);
            yield return playOriginalNode();
            DialogueSystem.instance.SetAuto(false);
            
            yield return DialogueSystem.instance.HandleSelection(options, (selectedOption) =>
            {
                pickedOption = selectedOption;
            });
            
            if(onCorrect != null && pickedOption.isCorrect)
               onCorrect();
            
            foreach (T node in pickedOption.dialogue)
            {
                yield return node.Play();
            }

            if (onWrong != null && !pickedOption.isCorrect)
                onWrong();

        } while (!pickedOption.isCorrect && loopIfWrong);
    }
}