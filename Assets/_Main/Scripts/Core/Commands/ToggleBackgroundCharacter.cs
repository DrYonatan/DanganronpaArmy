using System.Collections;
using UnityEditor;
using UnityEngine;

public enum VisibilityToggle
{
    Show,
    Hide,
    Delete
}

public class ToggleBackgroundCharacter : Command
{
    public Character character;
    public VisibilityToggle visibility;

    public override IEnumerator Execute()
    {
        switch (visibility)
        {
            case VisibilityToggle.Show:
                ImageScript.instance.ShowCharacterOnBackground(character);
                break;
            case VisibilityToggle.Hide:
                ImageScript.instance.HideCharacterOnBackground(character);
                break;
            case VisibilityToggle.Delete:
                ImageScript.instance.DeleteCharacterOnBackground(character);
                break;
        }

        yield return new WaitForSeconds(0.1f);
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        character =
            (Character)EditorGUILayout.ObjectField("character", character, typeof(Character),
                false);
        visibility =  (VisibilityToggle)EditorGUILayout.EnumPopup("visibility", visibility);
    }
#endif
}