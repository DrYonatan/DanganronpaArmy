using System;

[Serializable]
public abstract class Command
{
    public float height = 70;
    public abstract void Execute();
    
    public abstract void DrawGUI();
}
