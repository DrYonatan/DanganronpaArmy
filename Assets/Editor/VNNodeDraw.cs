using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Draw/Dialogue Node Draw")]
public class VNNodeDraw : DrawNode
{
    public override void DrawWindow(DialogueNode b)
    {
        b.nodeRect.height = 200;
        b.nodeRect.width = 500;
    }
}
