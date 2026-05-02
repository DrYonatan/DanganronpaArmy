using System.Collections;
using CHARACTERS;
using UnityEngine;

public class HideSpeaker : Command
{
    public override IEnumerator Execute()
    {
        VNCharacterManager.instance.HideCharacter(VNCharacterManager.instance.GetSpeakerObject(),0.5f);
        yield return new WaitForSeconds(0.5f);
    }
}
