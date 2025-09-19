using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Discussion Choice Node Draw")]
public class DiscussionChoiceNodeDraw : DiscussionNodeDraw
{
    public ChoiceLogicDraw choiceLogicDraw;

    public override void DrawWindow(DialogueNode b, float windowWidth, float windowHeight)
    {
        DiscussionChoiceNode node = (DiscussionChoiceNode)b;
        if (node != null)
        {
            base.DrawWindow(b, windowWidth, windowHeight);
            choiceLogicDraw.DrawLogic(node.choiceLogic);
        }
    }
}