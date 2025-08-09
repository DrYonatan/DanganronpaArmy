using UnityEditor;

public abstract class DialogueEditor : EditorWindow
{
    public abstract void SetContainer();

    public abstract void SetNewList();

    public abstract void AddNode(int index);
}
