using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Choice Logic Draw")]
public class ChoiceLogicDraw<T> : ScriptableObject where T : DialogueNode
{
    public void DrawLogic(ChoiceLogic<T> logic, Action<List<T>> open)
    {
        GUILayout.BeginHorizontal(GUILayout.Width(100));
        GUILayout.Label("Options");
        logic.loopIfWrong = GUILayout.Toggle(logic.loopIfWrong, "Loop if wrong");
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        
        for(int i = 0; i < logic.options.Count; i++)
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(100));
            GUILayout.BeginHorizontal();
            logic.options[i].isCorrect = GUILayout.Toggle(logic.options[i].isCorrect, "Correct");
            bool xButton = GUILayout.Button("X");
            GUILayout.EndHorizontal();

            if (xButton)
            {
                RemoveOption(logic, i);
            }
            else
            {
                logic.options[i].text = GUILayout.TextField(logic.options[i].text, GUILayout.Height(30));
                if (GUILayout.Button("Result dialogue"))
                {
                    open(logic.options[i].dialogue);
                } 
            }
            
            GUILayout.EndVertical();
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

    void AddOption(ChoiceLogic<T> logic)
    {
        logic.options.Insert(logic.options.Count, new Option<T>());
    }

    void RemoveOption(ChoiceLogic<T> logic, int index)
    {
        logic.options.RemoveAt(index);
    }
}