using System.Collections;
using UnityEditor;
using UnityEngine;

public class RemoveAnimatedImage : Command
{
    public float duration = 0.2f;
    public bool flash;

    public override IEnumerator Execute()
    {
        ImageScript.instance.RemoveAnimatedImage(duration, flash);
        yield return new WaitForSeconds(0.2f);
    }
    
    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        flash = EditorGUILayout.Toggle("Flash", flash);
        duration = EditorGUILayout.FloatField("Duration", duration);
    }
#endif
}
