using System.Collections;
using UnityEditor;
using UnityEngine;

public class ShowImage : Command
{
    public Sprite image;
    public bool flash;
    public override IEnumerator Execute()
    {
        ImageScript.instance?.Show(image, flash);
        yield return null;
    }
#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        image =
            (Sprite)EditorGUILayout.ObjectField("Image", image, typeof(Sprite), false);
        flash = EditorGUILayout.Toggle("Flash", flash);
    }
#endif
}