using System.Collections;
using CHARACTERS;

public class HideCharacter : Command
{
    public override IEnumerator Execute()
    {
        VNCharacterManager.instance.HideCharacter(VNCharacterManager.instance.GetSpeakerObject(),0.5f);
        yield return null;
    }
}
