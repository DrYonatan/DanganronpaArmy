using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Choice Node Draw")]

public class ChoiceNodeDraw: VNNodeDraw
{
    public ChoiceLogicDraw choiceLogicDraw;
    public Texture2D backgroundTexture;
    public override void DrawWindow(DialogueNode b, float windowWidth, float windowHeight)
    {
        VNChoiceNode node = (VNChoiceNode)b;
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
            choiceLogicDraw.DrawLogic(node.choiceLogic);
        }
    }
}