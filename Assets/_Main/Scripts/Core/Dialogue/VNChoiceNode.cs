using System;
using System.Collections;

[Serializable]
public class VNChoiceNode : DialogueNode
{
    public ChoiceLogic choiceLogic = new ChoiceLogic();

    public VNChoiceNode(DrawNode drawNode) : base(drawNode)
    {
        
    }

    public override IEnumerator Play()
    {
        yield return base.Play();
        yield return choiceLogic.Play();
    }
}