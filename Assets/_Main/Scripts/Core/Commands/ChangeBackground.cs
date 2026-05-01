using System.Collections;
using UnityEditor;
using UnityEngine;

public class ChangeBackground : Command
{
    public Sprite image;
    public override IEnumerator Execute()
    {
        ImageScript.instance.ShowBackground(image);
        yield return new WaitForSeconds(0.1f);
    }
#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        image =
            (Sprite)EditorGUILayout.ObjectField("Image", image, typeof(Sprite), false);
    }
#endif
}