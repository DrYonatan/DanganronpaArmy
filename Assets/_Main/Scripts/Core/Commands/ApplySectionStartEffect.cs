using System.Collections;
using DIALOGUE;
using UnityEditor;
using UnityEngine;

public class ApplySectionStartEffect : Command
{
    public string sectionTitle;
    public Color color = Color.green;

    public override IEnumerator Execute()
    {
        VNUIAnimator.instance.sectionStartEffect.background.color = color;
        VNUIAnimator.instance.sectionStartEffect.Animate(sectionTitle);
        DialogueSystem.instance.TextBoxDisappear();
        DialogueSystem.instance.TurnOnSingleTimeAuto();
        yield return new WaitForSeconds(2f);
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        sectionTitle = EditorGUILayout.TextField("Section Title", sectionTitle);
        color = EditorGUILayout.ColorField("Color", color);
    }
#endif
}
