using System.Collections;
using DIALOGUE;

public class RemoveCutscene : Command
{
    public override IEnumerator Execute()
    {
        yield return CutSceneManager.instance.Hide();
        
        DialogueSystem.instance.TextBoxDisappear();
        DialogueSystem.instance.SetTextBox(DialogueSystem.instance.defaultDialogueBoxAnimator);
        DialogueSystem.instance.TextBoxAppear();
        VNUIAnimator.instance.Appear();
    }
}
