using System.Collections;
using DIALOGUE;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class PlayCutscene : Command
{
    public VideoClip video;
    public bool persist;
    public override IEnumerator Execute()
    {
        DialogueSystem.instance.dialogueBoxAnimator.TextBoxDisappear();
        VNUIAnimator.instance.Disappear();
        
        ImageScript.instance.FadeToBlack(0.2f);
        yield return new WaitForSeconds(0.7f);
        ImageScript.instance.UnFadeToBlack(0f);
        
        yield return CutSceneManager.instance.PlayCutscene(video);
        if (!persist)
        {
            ImageScript.instance.FadeToBlack(0.2f);
            yield return new WaitForSeconds(0.2f);
            CutSceneManager.instance.Hide();
            ImageScript.instance.UnFadeToBlack(0.1f);
        }
        else
        {
            OverlayTextBoxManager.instance.SetAsTextBox();
        }
        
        DialogueSystem.instance.TextBoxAppear();
        VNUIAnimator.instance.Appear();
    }
    
    #if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        video =
            (VideoClip)EditorGUILayout.ObjectField("Evidence", video, typeof(VideoClip), false);
        persist = EditorGUILayout.Toggle("Persist", persist);
    }
    #endif
}
