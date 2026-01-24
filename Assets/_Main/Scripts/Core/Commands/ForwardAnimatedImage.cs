using System.Collections;

public class ForwardAnimatedImage : Command
{
    public override IEnumerator Execute()
    {
        yield return ImageScript.instance.ForwardAnimatedImage();
    }
}
