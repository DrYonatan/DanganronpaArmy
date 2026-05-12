using System.Collections;
using UnityEngine;

public class Ding : Command
{
    public override IEnumerator Execute()
    {
        ImageScript.instance.Flash(0.4f, Resources.Load<AudioClip>("Audio/Sound Effects/ding"));
        yield return new WaitForSeconds(0.4f);
    }
}