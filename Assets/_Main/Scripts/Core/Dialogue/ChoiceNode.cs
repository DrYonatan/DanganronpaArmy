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
public class ChoiceNode: DialogueNode
{
    public List<Option> options;
    public bool loopIfWrong = false;
    
    public ChoiceNode(DrawNode drawNode) : base(drawNode)
    {
        textData = new VNTextData();
        options = new List<Option>();
    }

    public override IEnumerator Play()
    {
        yield return base.Play();
        
        Option pickedOption = new Option();

        while (!pickedOption.isCorrect || !loopIfWrong)
        {
            pickedOption = options[0];
            foreach (DialogueNode node in pickedOption.dialogue)
            {
                yield return node.Play();
            }
        }
    }
}