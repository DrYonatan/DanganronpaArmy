using System.Collections;
using UnityEditor;

public class AddEvidence : Command
{
    public Evidence evidence;

    public override IEnumerator Execute()
    {
        yield return EvidenceManager.instance.AddEvidence(evidence);
    }
#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        evidence =
            (Evidence)EditorGUILayout.ObjectField("Evidence", evidence, typeof(Evidence), false);
    }
#endif
}