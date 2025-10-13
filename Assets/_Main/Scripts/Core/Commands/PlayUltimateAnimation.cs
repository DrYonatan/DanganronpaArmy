using System.Collections;
using DIALOGUE;
using UnityEditor;
using UnityEngine;


public class PlayUltimateAnimation : Command
{
    private Color backgroundColor;
    private Color nameColor;
    private Color descriptionColor;
    private Character character;
    private string nameText;
    private string descriptionText;

    private const string ultimatePrefabPath = "UI Objects/Ultimate Animation";
    private const string ultimateAnimationParentPath = "VN controller/Root/Canvas - Main/LAYERS/1 - Background";

    public override IEnumerator Execute()
    {
        DialogueSystem.instance.dialogueBoxAnimator.TextBoxDisappear();

        yield return new WaitForSeconds(0.2f);

        Transform parent = GameObject.Find(ultimateAnimationParentPath).transform;
        UltimateIntroductionAnimator ultimateAnimation =
            Object.Instantiate(Resources.Load<UltimateIntroductionAnimator>(ultimatePrefabPath), parent);
        ultimateAnimation.transform.localPosition = Vector3.zero;
        
        yield return ultimateAnimation.Play(character, backgroundColor, nameText, descriptionText, nameColor,
            descriptionColor);
        
        Object.Destroy(ultimateAnimation.gameObject);
        DialogueSystem.instance.dialogueBoxAnimator.TextBoxAppear();

    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        backgroundColor = EditorGUILayout.ColorField("Background Color", backgroundColor);
        nameText = EditorGUILayout.TextField("Name Text", nameText);
        nameColor = EditorGUILayout.ColorField("Name Color", nameColor);
        descriptionColor = EditorGUILayout.ColorField("Description Color", descriptionColor);
        descriptionText = EditorGUILayout.TextField("Description Text", descriptionText);
        character = EditorGUILayout.ObjectField("Character", character, typeof(Character), false) as Character;
    }
#endif
}