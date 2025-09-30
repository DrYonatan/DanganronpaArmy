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

    public IEnumerator OnCorrect()
    {
        TrialManager.instance.IncreaseHealth(0.5f);
        yield return new WaitForSeconds(1f);
        TrialManager.instance.barsAnimator.HideGlobalBars(0.2f);
    }

    public IEnumerator OnWrong()
    {
        TrialManager.instance.DecreaseHealth(1f);
        yield return UtilityNodesRuntimeBank.instance.nodesCollection.wrongAnswer.Play();
    }
    
}