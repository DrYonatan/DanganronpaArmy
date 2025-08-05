using System;

[Serializable]
public abstract class Command
{
    public float height = 70;
    public abstract void Execute();
    
    #if UNITY_EDITOR
    public abstract void DrawGUI();
    #endif
}
