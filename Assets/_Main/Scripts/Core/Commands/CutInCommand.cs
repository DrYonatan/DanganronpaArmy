using System.Collections;
using DIALOGUE;
using UnityEditor;
using UnityEngine;

public class CutInCommand : Command
{
    [SerializeField] private CharacterCutInWrapper cutInWrapper;

    public override IEnumerator Execute()
    {
        CharacterCutIn newCutIn = TrialDialogueManager.instance.animator.InstantiateCutIn(cutInWrapper.cutIn);
        ScreenShatterManager screenShatter =
            TrialDialogueManager.instance.animator.InstantiateScreenShatter(cutInWrapper.screenShatter);
        
        newCutIn.Animate();
        yield return new WaitForSeconds(newCutIn.duration + 0.5f);

        yield return screenShatter.ScreenShatter();
        
        ImageScript.instance.UnFadeToBlack(0.2f);
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        cutInWrapper =
            (CharacterCutInWrapper)EditorGUILayout.ObjectField("Cut In", cutInWrapper, typeof(CharacterCutInWrapper),
                false);
    }
#endif
}