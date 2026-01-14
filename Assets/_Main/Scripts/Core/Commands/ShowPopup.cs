using System.Collections;
using UnityEditor;
using UnityEngine;

public class ShowPopup : Command
{
    public Sprite image;
    public override IEnumerator Execute()
    {
        PopupAnimator.instance?.MakeImageAppear(image);
        yield return null;
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

