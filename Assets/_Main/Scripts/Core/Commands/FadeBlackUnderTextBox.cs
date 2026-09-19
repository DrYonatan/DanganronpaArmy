using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FadeBlackUnderTextBox : Command
{
    public bool fadeIn;
    public float duration = 0.2f;

    public override IEnumerator Execute()
    {
        ImageScript.instance.FadeUnderTextBoxBlack(fadeIn, duration);
        yield return duration;
    }
    
    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        fadeIn = EditorGUILayout.Toggle("Fade In", fadeIn);
        duration = EditorGUILayout.FloatField("Duration", duration);
    }
#endif
}
