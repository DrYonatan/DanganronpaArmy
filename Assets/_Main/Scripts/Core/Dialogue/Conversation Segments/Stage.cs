using System;
using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEngine;

[Serializable]
public class DebateSettings : ConversationSettings
{
    public Evidence[] evidences = new Evidence[5];
    public AudioClip audioClip;
}

[CreateAssetMenu(menuName ="Data/Stage")]
public class Stage : TrialSegment
{
    public List<DebateNode> dialogueNodes;
    
    public List<DiscussionNode> finishNodes;
    public DebateSettings settings = new ();

    public override void Play()
    {
        GameLoop.instance.PlayDebate(this);
    }
}
