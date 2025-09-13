using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Choice Node Draw")]

public class ChoiceNodeDraw: VNNodeDraw
{
    public override void DrawWindow(DialogueNode b, float windowWidth, float windowHeight)
    {
        ChoiceNode node = b as ChoiceNode;
        // base.DrawWindow(b, windowWidth, windowHeight);
        GUILayout.BeginVertical();
        if (node != null)
        {
            foreach (Option option in node!.options)
            {
                option.text = GUILayout.TextField(option.text);
            }

            if (GUILayout.Button("ADD OPTION"))
            {
                AddOption(node);
            }
        }
        GUILayout.EndVertical();

    }

    void AddOption(ChoiceNode node)
    {
        node.options.Insert(node.options.Count, new Option());
    }
}