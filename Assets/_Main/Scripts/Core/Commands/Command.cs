using System;

[Serializable]
public abstract class Command
{
    public abstract void Execute();
    
    #if UNITY_EDITOR
    public abstract void DrawGUI();
    #endif
}
