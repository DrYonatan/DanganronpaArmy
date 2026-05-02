using System.Collections;
using UnityEditor;
using UnityEngine;

public class ShowCharacter : Command
{
    public Character character;
    public override IEnumerator Execute()
    {
        yield return null;
    }
    
#if UNITY_EDITOR
    public override void DrawGUI()
    {
        character =
            (Character)EditorGUILayout.ObjectField("Image", character, typeof(Character),
                false);
    }
#endif
}
