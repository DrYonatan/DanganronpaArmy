
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Wait : Command
{
    public float time;
    public override IEnumerator Execute()
    {
        yield return new WaitForSeconds(time);
    }
    
    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        time = EditorGUILayout.FloatField(time);
    }
#endif
}
