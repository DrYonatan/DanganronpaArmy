using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DiscussionChoiceNode: DiscussionNode
{
    public ChoiceLogic<DiscussionNode> choiceLogic = new ChoiceLogic<DiscussionNode>();

    public DiscussionChoiceNode(DrawNode drawNode) : base(drawNode)
    {
        
    }

    public override IEnumerator Play()
    {
        yield return choiceLogic.Play(base.Play);
    }
}