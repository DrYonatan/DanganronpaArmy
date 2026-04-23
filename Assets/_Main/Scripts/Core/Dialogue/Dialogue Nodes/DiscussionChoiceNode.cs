using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DiscussionChoiceNode : DiscussionNode
{
    public bool isHp;
    public ChoiceLogic<DiscussionNode> choiceLogic = new ChoiceLogic<DiscussionNode>();

    public DiscussionChoiceNode(DrawNode drawNode) : base(drawNode)
    {
    }

    public override IEnumerator Play()
    {
        if (isHp)
        {
            TrialManager.instance.barsAnimator.ShowGlobalBars(0.2f);
            yield return choiceLogic.Play(base.Play, OnCorrect, OnWrong,
                () => !TrialDialogueManager.instance.isGameOvering);
        }

        else
        {
            yield return choiceLogic.Play(base.Play, null, null);
        }

        TrialManager.instance.barsAnimator.HideGlobalBars(0.2f);
    }

    public IEnumerator OnCorrect()
    {
        TrialManager.instance.IncreaseHealth(0.5f);
        yield return TrialDialogueManager.instance.gotItAnimator.Show();
    }

    public IEnumerator OnWrong()
    {
        TrialManager.instance.DecreaseHealthDefault(1f);
        yield return TrialDialogueManager.instance.PlayNodeList(UtilityNodesRuntimeBank.instance.nodesCollection.wrongAnswer);
    }
}