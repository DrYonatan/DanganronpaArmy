using System.Collections;
using UnityEditor;

public class PlayConversationCommand : Command
{
    public VNConversationSegment segment;

    public override IEnumerator Execute()
    {
        yield return VNNodePlayer.instance.RunSpecificNodes(segment.nodes);
        ImageScript.instance.Hide(false);
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        segment = (VNConversationSegment)EditorGUILayout.ObjectField("Segment", segment, typeof(VNConversationSegment), false);
    }
#endif
}
