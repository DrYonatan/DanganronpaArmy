using System;
using System.Collections;

[Serializable]
public class DiscussionChoiceNode: DiscussionNode
{
    public ChoiceLogic choiceLogic = new ChoiceLogic();

    public DiscussionChoiceNode(DrawNode drawNode) : base(drawNode)
    {
        
    }

    public override IEnumerator Play()
    {
        yield return base.Play();
        yield return choiceLogic.Play();
    }
}