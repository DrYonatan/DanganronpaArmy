using System.Collections;
using UnityEditor;

public class MarkFreeTimeCharacter : Command
{
    public Character character;
    public override IEnumerator Execute()
    {
        ((FreeTimeEvent)ProgressManager.instance.currentGameEvent).currentCharacter = character;
        yield return null;
    }
#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        character = (Character)EditorGUILayout.ObjectField("Character", character, typeof(Character), false);
    }
#endif
}