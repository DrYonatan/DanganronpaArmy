using System;
using System.Collections;

[Serializable]
public class VNChoiceNode : DialogueNode
{
    public ChoiceLogic<DialogueNode> choiceLogic = new ChoiceLogic<DialogueNode>();

    public VNChoiceNode(DrawNode drawNode) : base(drawNode)
    {
        
    }

    public override IEnumerator Play()
    {
        yield return base.Play();
        yield return choiceLogic.Play();
    }
}