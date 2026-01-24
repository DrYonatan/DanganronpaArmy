using System.Collections;

public class RemoveAnimatedImage : Command
{
    public override IEnumerator Execute()
    {
        ImageScript.instance.RemoveAnimatedImage();
        yield return null;
    }
}
