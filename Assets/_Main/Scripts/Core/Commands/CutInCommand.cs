using System.Collections;
using UnityEditor;
using UnityEngine;

public class CutInCommand : Command
{
    private GameObject cutIn;

    public override IEnumerator Execute()
    {
        CharacterCutIn newCutIn = TrialDialogueManager.instance.animator.InstantiateCutIn(cutIn);
        newCutIn.Animate();
        yield return newCutIn.duration + 0.5f;
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        cutIn = (GameObject)EditorGUILayout.ObjectField("Cut In", cutIn, typeof(GameObject), false);
    }
#endif
}