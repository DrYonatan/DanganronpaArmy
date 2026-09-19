using System.Collections;
using UnityEditor;
using UnityEngine;

public class ShowAnimatedImage : Command
{
    [SerializeField]
    private VNAnimatedImageWrapper imageWrapper;
    public float duration = 0.2f;
    public bool flash;


    public override IEnumerator Execute()
    {
        ImageScript.instance.CreateAnimatedImage(imageWrapper.image, duration, flash);
        yield return null;
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        imageWrapper =
            (VNAnimatedImageWrapper)EditorGUILayout.ObjectField("Image", imageWrapper, typeof(VNAnimatedImageWrapper),
                false);
        duration =  EditorGUILayout.FloatField("Duration", duration);
        flash = EditorGUILayout.Toggle("Flash", flash);
    }
#endif
}