using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[Serializable]
public abstract class Command
{
    public enum ExecuteTime
    {
        Before,
        Parallel,
        After
    }

    public ExecuteTime executeTime;
    public abstract IEnumerator Execute();
    
    #if UNITY_EDITOR
    public virtual void DrawGUI()
    {
        executeTime = (ExecuteTime)EditorGUILayout.EnumPopup(executeTime);
    }
    #endif
}
