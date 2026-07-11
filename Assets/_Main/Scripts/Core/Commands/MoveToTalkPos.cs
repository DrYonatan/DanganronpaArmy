using System.Collections;
using DIALOGUE;
using UnityEngine;

public class MoveToTalkPos : Command
{
    public string positionId;

    public override IEnumerator Execute()
    {
        Transform talkPosition = WorldManager.instance.talkPositions.Find((x) => x.name.Equals(positionId));

        if (talkPosition == null)
            talkPosition = WorldManager.instance.talkPositions[0];
        
        if (talkPosition == null)
            yield break;
        
        DialogueSystem.instance.TextBoxDisappear();
        CameraManager.instance.StartCoroutine(
            CameraManager.instance.RotateCameraTo(talkPosition.rotation, 0.5f));
        yield return CameraManager.instance.MoveCameraTo(talkPosition.position, 0.5f);
        CameraManager.instance.initialRotation = talkPosition.rotation;
    }
    
    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        positionId = GUILayout.TextField(positionId);
    }
#endif
}