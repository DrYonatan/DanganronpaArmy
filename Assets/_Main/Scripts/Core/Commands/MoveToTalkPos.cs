using System.Collections;
using UnityEngine;

public class MoveToTalkPos : Command
{
    public override IEnumerator Execute()
    {
        yield return CameraManager.instance.MoveCameraTo(WorldManager.instance.talkPosition.position, 0.5f);
    }
}