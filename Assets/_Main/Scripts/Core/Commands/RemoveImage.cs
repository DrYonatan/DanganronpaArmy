using System.Collections;
using UnityEditor;
using UnityEngine;

public class RemoveImage : Command
{
    public bool flash;
    public override IEnumerator Execute()
    {
        ImageScript.instance.Hide(flash);
        yield return null;
    }
#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        flash = EditorGUILayout.Toggle("Flash", flash);
    }
#endif
}