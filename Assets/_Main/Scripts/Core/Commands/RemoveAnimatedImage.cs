using System.Collections;
using UnityEditor;
using UnityEngine;

public class RemoveAnimatedImage : Command
{
    public float duration = 0.2f;
    public override IEnumerator Execute()
    {
        ImageScript.instance.RemoveAnimatedImage(duration);
        yield return new WaitForSeconds(0.2f);
    }
    
    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        duration = EditorGUILayout.FloatField("Duration", duration);
    }
#endif
}
