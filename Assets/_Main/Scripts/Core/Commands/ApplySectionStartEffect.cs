using System.Collections;
using DIALOGUE;
using UnityEditor;
using UnityEngine;

public class ApplySectionStartEffect : Command
{
    public string sectionTitle;

    public override IEnumerator Execute()
    {
        VNUIAnimator.instance.sectionStartEffect.Animate(sectionTitle);
        DialogueSystem.instance.TurnOnSingleTimeAuto();
        yield return null;
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        sectionTitle = EditorGUILayout.TextField("Section Title", sectionTitle);
    }
#endif
}
