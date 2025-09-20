using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Choice Logic Draw")]
public class ChoiceLogicDraw : ScriptableObject
{
    public void DrawLogic(ChoiceLogic logic)
    {
        GUILayout.BeginHorizontal(GUILayout.Width(100));
        GUILayout.Label("Options");
        logic.loopIfWrong = GUILayout.Toggle(logic.loopIfWrong, "Loop if wrong");
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();

        int index = 0;
        
        foreach (Option option in logic!.options)
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(100));
            GUILayout.BeginHorizontal();
            option.isCorrect = GUILayout.Toggle(option.isCorrect, "Correct");
            if (GUILayout.Button("X"))
            {
                RemoveOption(logic, index);
            }
            GUILayout.EndHorizontal();
            option.text = GUILayout.TextField(option.text, GUILayout.Height(30));
            GUILayout.Label("Result dialogue");
            GUILayout.EndVertical();
            index++;
        }

        GUIStyle plusStyle = GUIStyle.none;
        plusStyle.fontSize = 45;
        plusStyle.normal.background = new Texture2D(1, 1);
        plusStyle.alignment = TextAnchor.MiddleCenter;

        if (GUILayout.Button("+", plusStyle, GUILayout.Width(40), GUILayout.Height(40)))
        {
            AddOption(logic);
        }

        GUILayout.EndHorizontal();
    }

    void AddOption(ChoiceLogic logic)
    {
        logic.options.Insert(logic.options.Count, new Option());
    }

    void RemoveOption(ChoiceLogic logic, int index)
    {
        logic.options.RemoveAt(index);
    }
}