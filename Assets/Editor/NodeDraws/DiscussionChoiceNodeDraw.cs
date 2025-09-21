using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Discussion Choice Node Draw")]
public class DiscussionChoiceNodeDraw : DiscussionNodeDraw
{
    public ChoiceLogicDraw choiceLogicDraw;
    public Texture2D backgroundTexture;

    public override void DrawWindow(DialogueNode b, float windowWidth, float windowHeight)
    {
        DiscussionChoiceNode node = (DiscussionChoiceNode)b;
        if (node != null)
        {
            GUIStyle style = new GUIStyle();
            style.normal = new GUIStyleState();
            style.normal.background = backgroundTexture;
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize = 20;
            style.padding = new RectOffset(10, 10, 0, 0);
            GUILayout.Label("#Choice", style, GUILayout.ExpandWidth(false));
            base.DrawWindow(b, windowWidth, windowHeight);
            choiceLogicDraw.DrawLogic(node.choiceLogic, (nodes) =>
            {
                List<DiscussionNode> discussionNodes = new List<DiscussionNode>();
                if (nodes != null)
                {
                    foreach (DialogueNode dialogueNode in nodes)
                    {
                        discussionNodes.Insert(discussionNodes.Count, (DiscussionNode)dialogueNode);
                    }
                }

                ConversationEditor.Open(discussionNodes);
            });
        }
    }
}