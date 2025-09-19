using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Choice Logic Draw")]
public class ChoiceLogicDraw : ScriptableObject
{
    public void DrawLogic(ChoiceLogic logic)
    {
        GUILayout.BeginVertical();
        if (logic != null)
        {
            foreach (Option option in logic!.options)
            {
                option.text = GUILayout.TextField(option.text);
            }

            if (GUILayout.Button("ADD OPTION"))
            {
                AddOption(logic);
            }
        }
        GUILayout.EndVertical();

    }

    void AddOption(ChoiceLogic logic)
    {
        logic.options.Insert(logic.options.Count, new Option());
    }
}