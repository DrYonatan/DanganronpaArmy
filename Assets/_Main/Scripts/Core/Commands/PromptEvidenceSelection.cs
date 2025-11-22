using System;
using System.Collections;
using DIALOGUE;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PromptEvidenceSelection : Command
{
    public Evidence correctEvidence;
    public string question;

    public override IEnumerator Execute()
    {
        PlayerInputManager.instance.isPaused = true;
        yield return EvidenceManager.instance.evidenceMenu.SelectEvidence(question, OnEvidenceSelected);
        DialogueSystem.instance.TurnOnSingleTimeAuto();
    }

    IEnumerator OnEvidenceSelected(Evidence selectedEvidence)
    {
        PlayerInputManager.instance.isPaused = false;
        if (selectedEvidence.Name.Equals(correctEvidence.Name))
        {
            yield return OnCorrect();
        }
        else
        {
            yield return OnWrong();
        }
    }


    IEnumerator OnCorrect()
    {
        TrialManager.instance.IncreaseHealth(0.5f);
        yield return TrialDialogueManager.instance.gotItAnimator.Show();
        TrialManager.instance.barsAnimator.HideGlobalBars(0.2f);
    }

    IEnumerator OnWrong()
    {
        TrialManager.instance.DecreaseHealth(1f);
        yield return TrialDialogueManager.instance.RunNodes(UtilityNodesRuntimeBank.instance.nodesCollection.wrongAnswer);
        yield return Execute();
    }

#if UNITY_EDITOR
    public override void DrawGUI()
    {
        base.DrawGUI();
        correctEvidence =
            (Evidence)EditorGUILayout.ObjectField("Correct Evidence", correctEvidence, typeof(Evidence), false);
        question = EditorGUILayout.TextField("Question", question);
    }
#endif
}