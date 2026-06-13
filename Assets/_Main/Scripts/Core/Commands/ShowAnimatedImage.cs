using System.Collections;
using UnityEditor;
using UnityEngine;

public class ShowAnimatedImage : Command
{
    [SerializeField]
    private VNAnimatedImageWrapper imageWrapper;
    public float duration = 0.2f;

    public override IEnumerator Execute()
    {
        ImageScript.instance.CreateAnimatedImage(imageWrapper.image, duration);
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
    }
#endif
}