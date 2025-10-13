using System;
using System.Collections;
using DIALOGUE;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class PlayUltimateAnimation : Command
{
    public Color backgroundColor = Color.blue;
    public Color nameColor = Color.black;
    public Color descriptionColor = Color.black;
    public Character character;
    public string nameText;
    public string descriptionText;

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
        descriptionText = EditorGUILayout.TextField("Description Text", descriptionText);
        descriptionColor = EditorGUILayout.ColorField("Description Color", descriptionColor);
        character = EditorGUILayout.ObjectField("Character", character, typeof(Character), false) as Character;
    }
#endif
}