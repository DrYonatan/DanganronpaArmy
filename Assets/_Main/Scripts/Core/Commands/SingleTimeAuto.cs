using System.Collections;
using DIALOGUE;

public class SingleTimeAuto : Command
{
    public override IEnumerator Execute()
    {
        DialogueSystem.instance.TurnOnSingleTimeAuto();
        yield return null;
    }
}
