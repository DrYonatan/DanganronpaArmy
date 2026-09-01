using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEditor;
using UnityEngine;

public class TogglePauseAvailability : Command
{
    public bool availability;
    public override IEnumerator Execute()
    {
        PlayerInputManager.instance.pauseAvailable = availability;
        yield return null;
    }
    
    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        availability = EditorGUILayout.Toggle("Pause Available", availability);
    }
#endif
}
