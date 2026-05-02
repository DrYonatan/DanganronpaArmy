using System.Collections;
using UnityEditor;
using UnityEngine;

public class ShowAnimatedImage : Command
{
    [SerializeField]
    private VNAnimatedImageWrapper imageWrapper;

    public override IEnumerator Execute()
    {
        ImageScript.instance.CreateAnimatedImage(imageWrapper.image);
        yield return null;
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        imageWrapper =
            (VNAnimatedImageWrapper)EditorGUILayout.ObjectField("Image", imageWrapper, typeof(VNAnimatedImageWrapper),
                false);
    }
#endif
}