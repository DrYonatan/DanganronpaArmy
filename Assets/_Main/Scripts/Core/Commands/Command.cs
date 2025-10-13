using System;
using System.Collections;

[Serializable]
public abstract class Command
{
    public abstract IEnumerator Execute();
    
    #if UNITY_EDITOR
    public abstract void DrawGUI();
    #endif
}
