using System.Collections;
using UnityEditor;
using UnityEngine;

public class ShowPopup : Command
{
    public Sprite image;
    private const string IMAGE_ANIMATOR_PATH = "VN controller/Root/Canvas - Main/LAYERS/6 - Overlay/Popup Dialogue";
    public override IEnumerator Execute()
    {
        PopupAnimator animator = GameObject.Find(IMAGE_ANIMATOR_PATH)?.GetComponent<PopupAnimator>();
        animator?.MakeImageAppear(image);
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

