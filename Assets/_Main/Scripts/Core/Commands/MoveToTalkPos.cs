using System.Collections;

public class MoveToTalkPos : Command
{
    public override IEnumerator Execute()
    {
        CameraManager.instance.StartCoroutine(
            CameraManager.instance.RotateCameraTo(WorldManager.instance.talkPosition.rotation, 0.5f));
        yield return CameraManager.instance.MoveCameraTo(WorldManager.instance.talkPosition.position, 0.5f);
        CameraManager.instance.initialRotation =  WorldManager.instance.talkPosition.rotation;
    }
}