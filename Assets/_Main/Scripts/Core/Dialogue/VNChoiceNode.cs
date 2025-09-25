using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class VNChoiceNode : DialogueNode
{
    public ChoiceLogic<DialogueNode> choiceLogic = new ChoiceLogic<DialogueNode>();

    public VNChoiceNode(DrawNode drawNode) : base(drawNode)
    {
        
    }

    public override IEnumerator Play()
    {
        yield return choiceLogic.Play(base.Play);
    }
}