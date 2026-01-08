using System.Collections;
using UnityEngine;

public class RemovePopup : Command
{
    public override IEnumerator Execute()
    {
        PopupAnimator.instance?.MakeImageDisappear();
        yield return null;
    }
}