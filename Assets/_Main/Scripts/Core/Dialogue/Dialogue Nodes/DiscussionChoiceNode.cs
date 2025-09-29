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
        TrialManager.instance.barsAnimator.ShowGlobalBars(0.2f);
        yield return choiceLogic.Play(base.Play, OnCorrect, OnWrong);
    }

    public void OnCorrect()
    {
        TrialManager.instance.IncreaseHealth(0.5f);
    }

    public void OnWrong()
    {
        TrialManager.instance.DecreaseHealth(1f);
    }
}