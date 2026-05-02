using System.Collections;
using UnityEngine;

public class HideBackground : Command
{
    public override IEnumerator Execute()
    {
        ImageScript.instance.HideBackground(0.1f);
        yield return new WaitForSeconds(0.1f);
    }
}
