using System.Collections;
using CHARACTERS;
using UnityEngine;

public class HideSpeaker : Command
{
    public bool footStepsSound;
    public override IEnumerator Execute()
    {
        VNCharacterManager.instance.HideCharacter(VNCharacterManager.instance.GetSpeakerObject(),0.5f);
        if (footStepsSound)
        {
            SoundManager.instance.PlaySoundEffect(CameraManager.instance.footStepsSound);
        }
        yield return new WaitForSeconds(0.5f);
    }
    
    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        footStepsSound =  GUILayout.Toggle(footStepsSound, "footStepsSound");
    }
#endif
}
