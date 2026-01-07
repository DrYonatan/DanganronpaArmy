using System.Collections;
using UnityEngine;

public class RemovePopup : Command
{
    private const string IMAGE_ANIMATOR_PATH = "VN controller/Root/Canvas - Main/LAYERS/6 - Overlay/Popup Dialogue";
    public override IEnumerator Execute()
    {
        PopupAnimator animator = GameObject.Find(IMAGE_ANIMATOR_PATH)?.GetComponent<PopupAnimator>();
        animator?.MakeImageDisappear();
        yield return null;
    }
}