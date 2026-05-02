using System.Collections;
using UnityEngine;

public class RemoveAnimatedImage : Command
{
    public override IEnumerator Execute()
    {
        ImageScript.instance.RemoveAnimatedImage(0.2f);
        yield return new WaitForSeconds(0.2f);
    }
}
